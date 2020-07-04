using ErrorCentral.Application.Services;
using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
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
            var fakeOrderCmd = FakeLogErrorRequest(new Dictionary<string, object>
            { ["title"] = "Titulo" });

            _userRepositoryMock.Setup(svc => svc.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User>(It.IsAny<User>()));

            //Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var cltToken = new CancellationToken();
            Func<Task> act = async () => { await service.CreateAsync(fakeOrderCmd, cltToken); };

            //Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Invalid number of userId");
        }

        [Fact(DisplayName = "Create - Return False If LogError Is Not Persisted")]
        [Trait("Operation", "Create")]
        public async Task Create_handle_return_false_if_log_error_is_not_persisted()
        {

            var fakeOrderCmd = FakeLogErrorRequest(new Dictionary<string, object>
            { ["title"] = "Titulo" });

            _logErrorRepositoryMock.Setup(logErrorRepo => logErrorRepo.UnitOfWork.SaveChangesAsync(default(CancellationToken)))
                .Returns(Task.FromResult(1));

            _userRepositoryMock.Setup(svc => svc.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<User>(FakeUser()));

            //Act
            var service = new LogErrorService(_logErrorRepositoryMock.Object, _userRepositoryMock.Object);
            var cltToken = new CancellationToken();
            var result = await service.CreateAsync(fakeOrderCmd, cltToken);

            //Assert
            result
                .Should()
                .BeFalse();
        }

        private User FakeUser()
        {
            return new User();
        }

        private CreateLogErrorViewModel FakeLogErrorRequest(Dictionary<string, object> args = null)
        {
            return new CreateLogErrorViewModel(
                userId: args != null && args.ContainsKey("userId") ? (int)args["userId"] : 0,
                title: args != null && args.ContainsKey("title") ? (string)args["title"] : null,
                details: args != null && args.ContainsKey("details") ? (string)args["details"] : null,
                source: args != null && args.ContainsKey("source") ? (string)args["source"] : null,
                level: args != null && args.ContainsKey("level") ? (ELevel)args["level"] : ELevel.Debug,
                environment: args != null && args.ContainsKey("environment") ? (EEnvironment)args["environment"] : EEnvironment.Development);
        }
    }
}
