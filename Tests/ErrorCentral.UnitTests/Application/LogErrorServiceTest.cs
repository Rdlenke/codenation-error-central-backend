using ErrorCentral.Application.Services;
using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using FluentAssertions;
using Moq;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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
            var logError = FakeLogErrorRequest();

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
            var logError = FakeLogErrorRequest();

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.UnitOfWork.SaveChangesAsync(default(CancellationToken)))
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
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.UnitOfWork.SaveEntitiesAsync(default(CancellationToken)))
                .Returns(Task.FromResult(true));

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.RemoveAsync(id);

            // Assert
            _logErrorRepositoryMock.Verify(l => l.GetByIdAsync(id));
            _logErrorRepositoryMock.Verify(l => l.UnitOfWork.SaveEntitiesAsync(default(CancellationToken)));
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
            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.UnitOfWork.SaveEntitiesAsync(default(CancellationToken)))
                .Returns(Task.FromResult(false));

            // Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var result = await service.RemoveAsync(id);

            // Assert
            _logErrorRepositoryMock.Verify(l => l.GetByIdAsync(id));
            _logErrorRepositoryMock.Verify(l => l.UnitOfWork.SaveEntitiesAsync(default(CancellationToken)));
            result.Success.Should().BeFalse();
            result.Errors.Should().Equal(new[] { $"Error persisting database changes" });
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

        private CreateLogErrorViewModel FakeLogErrorRequest()
        {
            return new CreateLogErrorViewModel()
            {
                Title = "Run-time exception (line 8): Attempted to divide by zero.",
                Details = "[System.DivideByZeroException: Attempted to divide by zero.] \nat Program.Main() :line 8",
                Source = "http://production.com/",
                Level = ELevel.Error,
                Environment = EEnvironment.Production,
                UserId = 1
            };
        }
    }
}
