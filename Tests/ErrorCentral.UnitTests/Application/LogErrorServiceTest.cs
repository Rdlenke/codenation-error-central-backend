using ErrorCentral.Application.Services;
using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Application.ViewModels.Misc;
using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using ErrorCentral.Domain.SeedWork;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ErrorCentral.UnitTests.Builders.ViewModels;
using ErrorCentral.UnitTests.Builders.AggregatesModel;
using System;
using FluentAssertions.Extensions;

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

        [Fact(DisplayName = "Create - Response errors if UserId not be greater zero")]
        [Trait("Operation", "Create")]
        public async Task Create_handle_response_success_false_if_userId_is_not_be_greater_zero()
        {
            // Arrange
            var logError = new CreateLogErrorViewModelBuilder()
                .WithUserId(0)
                .Build();

            var expected = new Response<CreateLogErrorViewModel>(
                data: logError,
                success: false,
                errors: new[] { "UserId must be greater than 0" });

            
            //Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.CreateAsync(logError);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }
        
        [Fact(DisplayName = "Create - Response errors if model is not valid")]
        [Trait("Operation", "Create")]
        public async Task Create_handle_response_success_false_if_model_is_not_valid()
        {
            // Arrange
            var logError = new CreateLogErrorViewModel();

            var expected = new Response<CreateLogErrorViewModel>(
                data: logError,
                success: false,
                errors: new[] {
                    "UserId must be greater than 0",
                    "Title cannot be null",
                    "Title cannot be empty",
                    "Source cannot be null",
                    "Source cannot be empty",
                    "Level cannot be empty",
                    "Level Informed value cannot be assigned",
                    "Environment cannot be empty",
                    "Environment Informed value cannot be assigned"
                });

            
            //Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.CreateAsync(logError);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Create - Return False If User Is Not Found")]
        [Trait("Operation", "Create")]
        public async Task Create_handle_response_success_false_if_user_not_found()
        {
            // Arrange
            var logError = new CreateLogErrorViewModelBuilder().Build();

            _userRepositoryMock.Setup(svc => svc.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User>(null));

            var expected = new Response<CreateLogErrorViewModel>(
                data: logError,
                success: false,
                errors: new[] {
                    $"User with id {logError.UserId} not found"
                });

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.CreateAsync(logError);

            // Assert
            _userRepositoryMock.Verify(u => u.GetAsync(It.IsAny<int>()));
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Create - Return False If LogError Is Not Persisted")]
        [Trait("Operation", "Create")]
        public async Task Create_handle_response_success_false_if_log_error_is_not_persisted()
        {
            // Arrange
            var logError = new CreateLogErrorViewModelBuilder().Build();

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(false));

            _userRepositoryMock.Setup(svc => svc.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User>(new UserBuilder().Build()));

            var expected = new Response<CreateLogErrorViewModel>(
                data: logError,
                success: false,
                errors: new[] { $"Error persisting database changes" });

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.CreateAsync(logError);

            // Assert
            _userRepositoryMock.Verify(u => u.GetAsync(It.IsAny<int>()));
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Create - Return Success true")]
        [Trait("Operation", "Create")]
        public async Task Create_handle_response_success_true()
        {
            // Arrange
            var logError = new CreateLogErrorViewModelBuilder().Build();

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            _userRepositoryMock.Setup(svc => svc.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User>(new UserBuilder().Build()));

            var expected = new Response<CreateLogErrorViewModel>(
                data: logError,
                success: true);

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.CreateAsync(logError);

            // Assert
            _userRepositoryMock.Verify(u => u.GetAsync(It.IsAny<int>()));
            result
                .Should().BeEquivalentTo(expected);
        }

        [Theory(DisplayName = "Delete - Return Success false if log error not find")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [Trait("Operation", "Delete")]
        public async Task Remove_handle_return_fail_if_log_error_not_find(int id)
        {
            // Arrange
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetByIdAsync(id))
                .Returns(Task.FromResult<LogError>(null));

            var expected = new Response<int>(
                data: id,
                success: false,
                errors: new[] { $"object with id {id} not found" });

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.RemoveAsync(id);

            // Assert
            _logErrorRepositoryMock.Verify(l => l.GetByIdAsync(id));
            result
                .Should().BeEquivalentTo(expected);
        }

        [Theory(DisplayName = "Delete - Return Success true if log error removed")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [Trait("Operation", "Delete")]
        public async Task Remove_handle_return_success_if_log_error_removed(int id)
        {
            // Arrange
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetByIdAsync(id))
                .Returns(Task.FromResult(FakeLogError()));
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            var expected = new Response<int>(
                data: id,
                success: true);

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.RemoveAsync(id);

            // Assert
            _logErrorRepositoryMock.Verify(l => l.GetByIdAsync(id));
            _logErrorRepositoryMock.Verify(l => l.UnitOfWork.SaveEntitiesAsync(default));
            result
                .Should().BeEquivalentTo(expected);
        }

        [Theory(DisplayName = "Delete - Return Success true if log error not persisted")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [Trait("Operation", "Delete")]
        public async Task Remove_handle_return_success_if_log_error_not_persisted(int id)
        {
            // Arrange
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetByIdAsync(id))
                .Returns(Task.FromResult(FakeLogError()));
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(false));

            var expected = new Response<int>(
                data: id,
                success: false,
                errors: new[] { $"Error persisting database changes" });

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.RemoveAsync(id);

            // Assert
            _logErrorRepositoryMock.Verify(l => l.GetByIdAsync(id));
            _logErrorRepositoryMock.Verify(l => l.UnitOfWork.SaveEntitiesAsync(default));
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Get LogError By ID")]
        [Trait("Operation", "Get")]
        public async Task Get_log_error_by_id()
        {
            // Arrange
            _logErrorRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new LogErrorBuilder().Build()));
            var builderViewModel = new LogErrorDetailsViewModelBuilder();
            var expected = new Response<LogErrorDetailsViewModel>(data: builderViewModel.Build(), success: true);

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.GetLogError(It.IsAny<int>());

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Title.Should().BeEquivalentTo(builderViewModel.Title);
            result.Data.UserId.Should().Be(builderViewModel.UserId);
            result.Data.Details.Should().Be(builderViewModel.Details);
            result.Data.Level.Should().Be(builderViewModel.Level);
            result.Data.Environment.Should().Be(builderViewModel.Environment);
            result.Data.Source.Should().Be(builderViewModel.Source);
            result.Data.CreatedAt.Should()
                .BeAfter(1.Hours().Before(DateTime.UtcNow));
        }


        [Theory(DisplayName = "Get - Fail to Get LogError By ID")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Get_log_error_by_id_fail(int id)
        {
            // Arrange
            _logErrorRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .Returns(Task.FromResult<LogError>(null));

            var expected = new Response<LogErrorDetailsViewModel>(
                success: false,
                errors: new[] { $"There isn't a log error with {id}" });

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.GetLogError(id);

            // Assert
            result
                .Should()
                .BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return null if couldn't get logError")]
        public async void Get_return_null_if_couldnt_get_logerror()
        {
            // Arrange
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetAllUnarchivedAsync())
                .Returns(Task.FromResult<IList<LogError>>(null));

            var expected = new Response<List<ListLogErrorsViewModel>>(
                    success: false,
                    errors: new[] { "There are no errors to show" });

            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.Get(new GetLogErrorsQueryViewModel());

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return when there is no query")]
        public async void Get_return_when_there_is_no_query()
        {
            // Arrange
            IList<LogError> repositoryReturn = new List<LogError>() { 
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development)
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetAllUnarchivedAsync())
                .Returns(Task.FromResult(repositoryReturn));

            var viewModel = new ListLogErrorsViewModel(environment: repositoryReturn[0].Environment,
                                                        level: repositoryReturn[0].Level,
                                                        source: repositoryReturn[0].Source,
                                                        title: repositoryReturn[0].Title,
                                                        filed: repositoryReturn[0].Filed,
                                                        userId: repositoryReturn[0].UserId,
                                                        details: repositoryReturn[0].Details,
                                                        events: 1);

            var list = new List<ListLogErrorsViewModel>() { viewModel };

            var expected = new Response<List<ListLogErrorsViewModel>>(data: list, success: true, errors: null);

            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.Get(new GetLogErrorsQueryViewModel());

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return filtered by Environment")]
        public async void Get_return_filtered_by_environment()
        {
            // Arrange
            IList<LogError> repositoryReturn = new List<LogError>() {
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Homologation),
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetAllUnarchivedAsync())
                .Returns(Task.FromResult(repositoryReturn));

            var listLogErrors = repositoryReturn
                .Select(x => new ListLogErrorsViewModel(environment: x.Environment,
                                                        level: x.Level,
                                                        source: x.Source,
                                                        title: x.Title,
                                                        userId: x.UserId,
                                                        filed: x.Filed,
                                                        details: x.Details,
                                                        events: CountEvents(x, repositoryReturn))).ToList();

            var obtained = listLogErrors.Select(x => x).Where(x => x.Environment == EEnvironment.Development).ToList();

            var expected = new Response<List<ListLogErrorsViewModel>>(data: obtained, success: true, errors: null);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel();
            query.Environment = EEnvironment.Development;


            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.Get(query);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return filtered by search (level)")]
        public async void Get_return_filtered_by_search_default()
        {
            // Arrange
            IList<LogError> repositoryReturn = new List<LogError>() {
                new LogError(1, "Title", "Dotails", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Error, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Homologation),
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetAllUnarchivedAsync())
                .Returns(Task.FromResult(repositoryReturn));

            var listLogErrors = repositoryReturn
                .Select(x => new ListLogErrorsViewModel(environment: x.Environment,
                                                        level: x.Level,
                                                        source: x.Source,
                                                        title: x.Title,
                                                        filed: x.Filed,
                                                        userId: x.UserId,
                                                        details: x.Details,
                                                        events: CountEvents(x, repositoryReturn))).ToList();

            var obtained = listLogErrors.Select(x => x).Where(x => x.Details.Equals("Details")).ToList();

            var expected = new Response<List<ListLogErrorsViewModel>>(data: obtained, success: true, errors: null);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel();
            query.Search = "Details";

            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.Get(query);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return filtered by search (level)")]
        public async void Get_return_filtered_by_search_level()
        {
            // Arrange
            IList<LogError> repositoryReturn = new List<LogError>() {
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Error, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Homologation),
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetAllUnarchivedAsync())
                .Returns(Task.FromResult(repositoryReturn));

            var listLogErrors = repositoryReturn
                .Select(x => new ListLogErrorsViewModel(environment: x.Environment,
                                                        level: x.Level,
                                                        source: x.Source,
                                                        title: x.Title,
                                                        filed: x.Filed,
                                                        userId: x.UserId,
                                                        details: x.Details,
                                                        events: CountEvents(x, repositoryReturn))).ToList();

            var obtained = listLogErrors.Select(x => x).Where(x => x.Level == ELevel.Debug).ToList();

            var expected = new Response<List<ListLogErrorsViewModel>>(data: obtained, success: true, errors: null);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel();
            query.Search = "Debug";
            query.SearchBy = ESearchBy.LEVEL;


            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.Get(query);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return filtered by search (details)")]
        public async void Get_return_filtered_by_search_details()
        {
            // Arrange
            IList<LogError> repositoryReturn = new List<LogError>() {
                new LogError(1, "Title", "Dotails", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Error, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Homologation),
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetAllUnarchivedAsync())
                .Returns(Task.FromResult(repositoryReturn));

            var listLogErrors = repositoryReturn
                .Select(x => new ListLogErrorsViewModel(environment: x.Environment,
                                                        level: x.Level,
                                                        source: x.Source,
                                                        title: x.Title,
                                                        userId: x.UserId,
                                                        filed: x.Filed,
                                                        details: x.Details,
                                                        events: CountEvents(x, repositoryReturn))).ToList();

            var obtained = listLogErrors.Select(x => x).Where(x => x.Details.Equals("Dotails")).ToList();

            var expected = new Response<List<ListLogErrorsViewModel>>(data: obtained, success: true, errors: null);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel();
            query.Search = "Dotails";
            query.SearchBy = ESearchBy.DETAILS;


            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.Get(query);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return filtered by search (source)")]
        public async void Get_return_filtered_by_search_source()
        {
            // Arrange
            IList<LogError> repositoryReturn = new List<LogError>() {
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "127.0.1.1", ELevel.Error, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Homologation),
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetAllUnarchivedAsync())
                .Returns(Task.FromResult(repositoryReturn));

            var listLogErrors = repositoryReturn
                .Select(x => new ListLogErrorsViewModel(environment: x.Environment,
                                                        level: x.Level,
                                                        source: x.Source,
                                                        title: x.Title,
                                                        filed: x.Filed,
                                                        userId: x.UserId,
                                                        details: x.Details,
                                                        events: CountEvents(x, repositoryReturn))).ToList();

            var obtained = listLogErrors.Select(x => x).Where(x => x.Source.Equals("127.0.1.1")).ToList();

            var expected = new Response<List<ListLogErrorsViewModel>>(data: obtained, success: true, errors: null);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel();
            query.Search = "127.0.1.1";
            query.SearchBy = ESearchBy.SOURCE;


            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.Get(query);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return sorted (events)")]
        public async void Get_return_sorted_events()
        {
            // Arrange
            IList<LogError> repositoryReturn = new List<LogError>() {
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "127.0.1.1", ELevel.Error, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Homologation),
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetAllUnarchivedAsync())
                .Returns(Task.FromResult(repositoryReturn));

            var listLogErrors = repositoryReturn
                .Select(x => new ListLogErrorsViewModel(environment: x.Environment,
                                                        level: x.Level,
                                                        filed: x.Filed,
                                                        source: x.Source,
                                                        title: x.Title,
                                                        userId: x.UserId,
                                                        details: x.Details,
                                                        events: CountEvents(x, repositoryReturn))).ToList();

            var obtained = listLogErrors.Select(x => x).OrderByDescending(x => x.Events).ToList();

            var expected = new Response<List<ListLogErrorsViewModel>>(data: obtained, success: true, errors: null);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel();
            query.SortBy = ESortBy.FREQUENCY;

            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.Get(query);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Get - Return sorted (level)")]

        public async void Get_return_sorted_level()
        {
            // Arrange
            IList<LogError> repositoryReturn = new List<LogError>() {
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "127.0.1.1", ELevel.Error, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Homologation),
            };

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetAllUnarchivedAsync())
                .Returns(Task.FromResult(repositoryReturn));

            var listLogErrors = repositoryReturn
                .Select(x => new ListLogErrorsViewModel(environment: x.Environment,
                                                        level: x.Level,
                                                        source: x.Source,
                                                        filed: x.Filed,
                                                        title: x.Title,
                                                        userId: x.UserId,
                                                        details: x.Details,
                                                        events: CountEvents(x, repositoryReturn))).ToList();

            var obtained = listLogErrors.Select(x => x).OrderByDescending(x => x.Level).ToList();

            var expected = new Response<List<ListLogErrorsViewModel>>(data: obtained, success: true, errors: null);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel();
            query.SortBy = ESortBy.LEVEL;

            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.Get(query);

            // Assert
            result
                .Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Archived - Get all Archived Logs")]
        public async Task Get_archived()
        {
            List<LogError> logErrors = new List<LogError>() {
                new LogError(1, "Title", "Details", "Source", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "127.0.1.1", ELevel.Debug, EEnvironment.Development),
                new LogError(1, "Title", "Details", "Source", ELevel.Error, EEnvironment.Development),
            };

            logErrors[0].Archive();
            logErrors[1].Archive();

            _logErrorRepositoryMock.Setup(x => x.GetArchivedAsync())
                .Returns(Task.FromResult(logErrors.Take(2).ToList()));

            var listLogErrors = logErrors.Where(x => x.Filed == true)
                .Select(x => new ListLogErrorsViewModel(environment: x.Environment,
                                                        level: x.Level,
                                                        source: x.Source,
                                                        filed: x.Filed,
                                                        title: x.Title,
                                                        userId: x.UserId,
                                                        details: x.Details,
                                                        events: CountEvents(x, logErrors))).ToList();

            Response<List<ListLogErrorsViewModel>> response = new Response<List<ListLogErrorsViewModel>>(data: listLogErrors, success: true, errors: null);

            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.GetArchived();

            // Assert
            result
                .Should().BeEquivalentTo(response);
        }


        [Fact(DisplayName = "Archived - Fail to get all Logs")]
        public async Task Get_archived_fail()
        {
            _logErrorRepositoryMock.Setup(x => x.GetArchivedAsync())
                .Returns(Task.FromResult<List<LogError>>(null));

            Response<List<ListLogErrorsViewModel>> response = new Response<List<ListLogErrorsViewModel>>(success: false, errors: new[] { "There are no errors to show" });

            // Act
            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.GetArchived();

            // Assert
            result
                .Should().BeEquivalentTo(response);
        }

        [Fact(DisplayName = "Archived - Unarchive log")]
        public async Task Unarchive_success()
        {
            LogError logError = new LogErrorBuilder().Build();

            _logErrorRepositoryMock.Setup(x => x.GetByIdAsync(logError.Id))
                .Returns(Task.FromResult(logError));

            _logErrorRepositoryMock.Setup(x => x.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            ListLogErrorsViewModel viewModel = new ListLogErrorsViewModel(
                userId: logError.UserId,
                title: logError.Title,
                level: logError.Level,
                environment: logError.Environment,
                source: logError.Source,
                details: logError.Details,
                events: 1,
                filed: false
            );

            Response<int> response = new Response<int>(success: true, errors: null, data: logError.Id);

            LogErrorService service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.UnarchiveAsync(logError.Id);

            result
                .Should().BeEquivalentTo(response);
        }

        [Theory(DisplayName = "Archive - Return Success false if log error not find")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [Trait("Operation", "Patch")]
        public async Task Patch_handle_return_fail_if_log_error_not_find(int id)
        {
            // Arrange
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetByIdAsync(id))
                .Returns(Task.FromResult<LogError>(null));

            var expected = new Response<int>(
                data: id,
                success: false,
                errors: new[] { $"object with id {id} not found" });

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.ArchiveAsync(id);

            // Assert
            _logErrorRepositoryMock.Verify(l => l.GetByIdAsync(id));
            result
                .Should().BeEquivalentTo(expected);
        }

        [Theory(DisplayName = "Archive - Return Success true if log error filed")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [Trait("Operation", "Patch")]
        public async Task Archive_handle_return_success_if_log_error_filed(int id)
        {
            // Arrange
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetByIdAsync(id))
                .Returns(Task.FromResult(FakeLogError()));
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            var expected = new Response<int>(
                data: id,
                success: true);

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.ArchiveAsync(id);

            // Assert
            _logErrorRepositoryMock.Verify(l => l.GetByIdAsync(id));
            _logErrorRepositoryMock.Verify(l => l.UnitOfWork.SaveEntitiesAsync(default));
            result
                .Should().BeEquivalentTo(expected);
        }

        [Theory(DisplayName = "Archive - Return Success true if log error not persisted")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [Trait("Operation", "Patch")]
        public async Task Archive_handle_return_success_if_log_error_not_persisted(int id)
        {
            // Arrange
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.GetByIdAsync(id))
                .Returns(Task.FromResult(FakeLogError()));
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(false));

            var expected = new Response<int>(
                data: id,
                success: false,
                errors: new[] { $"Error persisting database changes" });

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.ArchiveAsync(id);

            // Assert
            _logErrorRepositoryMock.Verify(l => l.GetByIdAsync(id));
            _logErrorRepositoryMock.Verify(l => l.UnitOfWork.SaveEntitiesAsync(default));
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

        private int CountEvents(LogError logError, IList<LogError> listAllLogErrors)
        {
            var list = listAllLogErrors
                .Where(x => x.Details == logError.Details &&
                       x.Environment == logError.Environment &&
                       x.Title == logError.Title &&
                       x.Source == logError.Source &&
                       x.Level == logError.Level).ToList();

            return list.Count;
        }
    }
}
