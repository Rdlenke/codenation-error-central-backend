using ErrorCentral.Application.Services;
using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Application.ViewModels.Misc;
using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using ErrorCentral.Domain.SeedWork;
using FluentAssertions;
using FluentAssertions.Common;
using Moq;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ErrorCentral.UnitTests.Builders.ViewModels;

namespace ErrorCentral.UnitTests.Application
{
    public class LogErrorServiceTest
    {
        private readonly Mock<ILogErrorRepository> _logErrorRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public LogErrorServiceTest()
        {
            _logErrorRepositoryMock = new Mock<ILogErrorRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
        }

        [Fact(DisplayName = "Create - Throw Exception If UserId Not Found")]
        [Trait("Operation", "Create")]
        public void Create_handle_throw_exception_if_userId_not_found()
        {
            // Arrange
            var logError = new CreateLogErrorViewModelBuilder().Build();

            _userRepositoryMock.Setup(svc => svc.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User>(It.IsAny<User>()));

            //Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var cltToken = new CancellationToken();
            Func<Task> act = async () => { await service.CreateAsync(logError, cltToken); };

            //Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Invalid number of userId");
        }

        [Fact(DisplayName = "Create - Return False If LogError Is Not Persisted")]
        [Trait("Operation", "Create")]
        public async Task Create_handle_return_false_if_log_error_is_not_persisted()
        {
            // Arrange
            var logError = new CreateLogErrorViewModelBuilder().Build();

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.UnitOfWork.SaveChangesAsync(default))
                .Returns(Task.FromResult(1));

            _userRepositoryMock.Setup(svc => svc.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User>(FakeUser()));

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var cltToken = new CancellationToken();
            var result = await service.CreateAsync(logError, cltToken);

            // Assert
            result
                .Should()
                .BeFalse();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Remove_handle_return_fail_if_log_error_not_find(int id)
        {
            // Arrange
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetByIdAsync(id))
                .Returns(Task.FromResult<LogError>(null));

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.RemoveAsync(id);

            // Assert
            _logErrorRepositoryMock.Verify(l => l.GetByIdAsync(id));
            result.Success.Should().BeFalse();
            result.Errors.Should().Equal(new[] { $"object with id {id} not found" });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Remove_handle_return_success_if_log_error_removed(int id)
        {
            // Arrange
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetByIdAsync(id))
                .Returns(Task.FromResult(FakeLogError()));
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.RemoveAsync(id);

            // Assert
            _logErrorRepositoryMock.Verify(l => l.GetByIdAsync(id));
            _logErrorRepositoryMock.Verify(l => l.UnitOfWork.SaveEntitiesAsync(default));
            result.Success.Should().BeTrue();
            result.Errors.Should().BeNull();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Remove_handle_return_success_if_log_error_not_persisted(int id)
        {
            // Arrange
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetByIdAsync(id))
                .Returns(Task.FromResult(FakeLogError()));
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(false));

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.RemoveAsync(id);

            // Assert
            _logErrorRepositoryMock.Verify(l => l.GetByIdAsync(id));
            _logErrorRepositoryMock.Verify(l => l.UnitOfWork.SaveEntitiesAsync(default));
            result.Success.Should().BeFalse();
            result.Errors.Should().Equal(new[] { $"Error persisting database changes" });
        }

        [Fact(DisplayName = "Get - Get LogError By ID")]
        public async Task Get_log_error_by_id()
        {
            // Arrange
            LogError logError = new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development);
            _logErrorRepositoryMock.Setup(x => x.GetById(1))
                .Returns(Task.FromResult(logError));

            LogErrorDetailsViewModel viewModel = new LogErrorDetailsViewModel(
                userId: logError.UserId,
                title: logError.Title,
                details: logError.Details,
                level: logError.Level,
                environment: logError.Environment,
                createdAt: logError.CreatedAt,
                source: logError.Source
            );

            Response<LogErrorDetailsViewModel> expected = new Response<LogErrorDetailsViewModel>(data: viewModel, success: true, errors: null);

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.GetLogError(1);

            // Assert
            result
                .Should()
                .BeEquivalentTo(expected);

        }


        [Fact(DisplayName = "Get - Fail to Get LogError By ID")]
        public async Task Get_log_error_by_id_fail()
        {
            // Arrange
            _logErrorRepositoryMock.Setup(x => x.GetById(1))
                .Returns(Task.FromResult<LogError>(null));

            Response<LogErrorDetailsViewModel> expected = new Response<LogErrorDetailsViewModel>(
                    success: false,
                    errors: new[] { $"There isn't a log error with {1}" });

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.GetLogError(1);

            // Assert
            result
                .Should()
                .BeEquivalentTo(expected);

        }

        [Fact(DisplayName = "Get - Return null if couldn't get logError")]
        public void Get_return_null_if_couldnt_get_logerror()
        {
            // Arrange
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetList())
                .Returns<List<LogError>>(null);

            var expected = new Response<List<ListLogErrorsViewModel>>(
                    success: false,
                    errors: new[] { "There are no errors to show" });

            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = service.Get(new GetLogErrorsQueryViewModel());

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return when there is no query")]
        public void Get_return_when_there_is_no_query()
        {
            // Arrange
            List<LogError> repositoryReturn = new List<LogError>() { 
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development)
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetList())
                .Returns(repositoryReturn);

            var viewModel = new ListLogErrorsViewModel(environment: repositoryReturn[0].Environment,
                                                        level: repositoryReturn[0].Level,
                                                        source: repositoryReturn[0].Source,
                                                        title: repositoryReturn[0].Title,
                                                        userId: repositoryReturn[0].UserId,
                                                        details: repositoryReturn[0].Details,
                                                        events: 1);

            var list = new List<ListLogErrorsViewModel>() { viewModel };

            var expected = new Response<List<ListLogErrorsViewModel>>(data: list, success: true, errors: null);

            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = service.Get(new GetLogErrorsQueryViewModel());

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return filtered by Environment")]
        public void Get_return_filtered_by_environment()
        {
            // Arrange
            List<LogError> repositoryReturn = new List<LogError>() {
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Homologation),
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetList())
                .Returns(repositoryReturn);

            var listLogErrors = repositoryReturn
                .GroupBy(x => new { x.Environment, x.Level, x.Title, x.Source, x.UserId, x.Details })
                .Select(x => new ListLogErrorsViewModel(environment: x.Key.Environment,
                                                        level: x.Key.Level,
                                                        source: x.Key.Source,
                                                        title: x.Key.Title,
                                                        userId: x.Key.UserId,
                                                        details: x.Key.Details,
                                                        events: x.Count()));

            var obtained = listLogErrors.Select(x => x).Where(x => x.Environment == EEnvironment.Development).ToList();

            var expected = new Response<List<ListLogErrorsViewModel>>(data: obtained, success: true, errors: null);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel();
            query.Environment = EEnvironment.Development;


            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = service.Get(query);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return filtered by search (level)")]
        public void Get_return_filtered_by_search_default()
        {
            // Arrange
            List<LogError> repositoryReturn = new List<LogError>() {
                new LogError(1, "Title", "Dotails", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Error, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Homologation),
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetList())
                .Returns(repositoryReturn);

            var listLogErrors = repositoryReturn
                .GroupBy(x => new { x.Environment, x.Level, x.Title, x.Source, x.UserId, x.Details })
                .Select(x => new ListLogErrorsViewModel(environment: x.Key.Environment,
                                                        level: x.Key.Level,
                                                        source: x.Key.Source,
                                                        title: x.Key.Title,
                                                        userId: x.Key.UserId,
                                                        details: x.Key.Details,
                                                        events: x.Count()));

            var obtained = listLogErrors.Select(x => x).Where(x => x.Details.Equals("Details")).ToList();

            var expected = new Response<List<ListLogErrorsViewModel>>(data: obtained, success: true, errors: null);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel();
            query.Search = "Details";

            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = service.Get(query);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return filtered by search (level)")]
        public void Get_return_filtered_by_search_level()
        {
            // Arrange
            List<LogError> repositoryReturn = new List<LogError>() {
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Error, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Homologation),
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetList())
                .Returns(repositoryReturn);

            var listLogErrors = repositoryReturn
                .GroupBy(x => new { x.Environment, x.Level, x.Title, x.Source, x.UserId, x.Details })
                .Select(x => new ListLogErrorsViewModel(environment: x.Key.Environment,
                                                        level: x.Key.Level,
                                                        source: x.Key.Source,
                                                        title: x.Key.Title,
                                                        userId: x.Key.UserId,
                                                        details: x.Key.Details,
                                                        events: x.Count()));

            var obtained = listLogErrors.Select(x => x).Where(x => x.Level == ELevel.Debug).ToList();

            var expected = new Response<List<ListLogErrorsViewModel>>(data: obtained, success: true, errors: null);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel();
            query.Search = "Debug";
            query.SearchBy = ESearchBy.LEVEL;


            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = service.Get(query);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return filtered by search (details)")]
        public void Get_return_filtered_by_search_details()
        {
            // Arrange
            List<LogError> repositoryReturn = new List<LogError>() {
                new LogError(1, "Title", "Dotails", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Error, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Homologation),
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetList())
                .Returns(repositoryReturn);

            var listLogErrors = repositoryReturn
                .GroupBy(x => new { x.Environment, x.Level, x.Title, x.Source, x.UserId, x.Details })
                .Select(x => new ListLogErrorsViewModel(environment: x.Key.Environment,
                                                        level: x.Key.Level,
                                                        source: x.Key.Source,
                                                        title: x.Key.Title,
                                                        userId: x.Key.UserId,
                                                        details: x.Key.Details,
                                                        events: x.Count()));

            var obtained = listLogErrors.Select(x => x).Where(x => x.Details.Equals("Dotails")).ToList();

            var expected = new Response<List<ListLogErrorsViewModel>>(data: obtained, success: true, errors: null);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel();
            query.Search = "Dotails";
            query.SearchBy = ESearchBy.DETAILS;


            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = service.Get(query);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return filtered by search (source)")]
        public void Get_return_filtered_by_search_source()
        {
            // Arrange
            List<LogError> repositoryReturn = new List<LogError>() {
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "127.0.1.1", ELevel.Error, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Homologation),
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetList())
                .Returns(repositoryReturn);

            var listLogErrors = repositoryReturn
                .GroupBy(x => new { x.Environment, x.Level, x.Title, x.Source, x.UserId, x.Details })
                .Select(x => new ListLogErrorsViewModel(environment: x.Key.Environment,
                                                        level: x.Key.Level,
                                                        source: x.Key.Source,
                                                        title: x.Key.Title,
                                                        userId: x.Key.UserId,
                                                        details: x.Key.Details,
                                                        events: x.Count()));

            var obtained = listLogErrors.Select(x => x).Where(x => x.Source.Equals("127.0.1.1")).ToList();

            var expected = new Response<List<ListLogErrorsViewModel>>(data: obtained, success: true, errors: null);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel();
            query.Search = "127.0.1.1";
            query.SearchBy = ESearchBy.SOURCE;


            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = service.Get(query);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return sorted (events)")]
        public void Get_return_sorted_events()
        {
            // Arrange
            List<LogError> repositoryReturn = new List<LogError>() {
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "127.0.1.1", ELevel.Error, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Homologation),
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetList())
                .Returns(repositoryReturn);

            var listLogErrors = repositoryReturn
                .GroupBy(x => new { x.Environment, x.Level, x.Title, x.Source, x.UserId, x.Details })
                .Select(x => new ListLogErrorsViewModel(environment: x.Key.Environment,
                                                        level: x.Key.Level,
                                                        source: x.Key.Source,
                                                        title: x.Key.Title,
                                                        userId: x.Key.UserId,
                                                        details: x.Key.Details,
                                                        events: x.Count()));

            var obtained = listLogErrors.Select(x => x).OrderByDescending(x => x.Events).ToList();

            var expected = new Response<List<ListLogErrorsViewModel>>(data: obtained, success: true, errors: null);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel();
            query.SortBy = ESortBy.FREQUENCY;

            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = service.Get(query);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return sorted (level)")]

        public void Get_return_sorted_level()
        {
            // Arrange
            List<LogError> repositoryReturn = new List<LogError>() {
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "127.0.1.1", ELevel.Error, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Homologation),
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetList())
                .Returns(repositoryReturn);

            var listLogErrors = repositoryReturn
                .GroupBy(x => new { x.Environment, x.Level, x.Title, x.Source, x.UserId, x.Details })
                .Select(x => new ListLogErrorsViewModel(environment: x.Key.Environment,
                                                        level: x.Key.Level,
                                                        source: x.Key.Source,
                                                        title: x.Key.Title,
                                                        userId: x.Key.UserId,
                                                        details: x.Key.Details,
                                                        events: x.Count()));

            var obtained = listLogErrors.Select(x => x).OrderByDescending(x => x.Level).ToList();

            var expected = new Response<List<ListLogErrorsViewModel>>(data: obtained, success: true, errors: null);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel();
            query.SortBy = ESortBy.LEVEL;

            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = service.Get(query);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        private LogError FakeLogError()
        {
            return new LogError(
                userId: 1,
                title: "Run-time exception (line 8): Attempted to divide by zero.",
                details: "[System.DivideByZeroException: Attempted to divide by zero.] \nat Program.Main() :line 8",
                source: "http://production.com/",
                level: ELevel.Error,
                environment: EEnvironment.Production
            );

        }

        private User FakeUser()
        {
            return new User(
                password: "12345",
                firstName: "João",
                lastName: "Alves",
                email: "joao@email.com"
            );
        }
    }
}
