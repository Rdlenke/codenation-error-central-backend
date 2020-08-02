using ErrorCentral.Application.Services;
using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using FluentAssertions;
using Moq;
using System;
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
