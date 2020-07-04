using ErrorCentral.API.v1.Controllers;
using ErrorCentral.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;
using ErrorCentral.Application.ViewModels.LogError;
using FluentAssertions;
using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using System.Threading;

namespace ErrorCentral.UnitTests.API
{
    public class LogErrorWebApiTest
    {
        private readonly Mock<ILogErrorService> _logErrorServiceMock;
        private readonly Mock<ILogger<LogErrorsController>> _loggerMock;

        public LogErrorWebApiTest()
        {
            _logErrorServiceMock = new Mock<ILogErrorService>();
            _loggerMock = new Mock<ILogger<LogErrorsController>>();
        }

        [Fact]
        [Trait("POST - Operation", "Create")]
        public async Task Create_log_error_success()
        {
            //Arrange
            var logError = new CreateLogErrorViewModel(
                userId: 1,
                title: "fakeTitle",
                details: "fakeDetails",
                source: "localhost",
                level: ELevel.Debug,
                environment: EEnvironment.Development);

            _logErrorServiceMock.Setup(x => x.CreateAsync(It.IsAny<CreateLogErrorViewModel>(), default))
                .Returns(Task.FromResult(true));

            //Act
            var orderController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await orderController.CreateLogErrorAsync(logError) as OkResult;

            //Assert
            actionResult.StatusCode.Should()
                .Be((int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        [Trait("POST - Operation", "Create")]
        public async Task Create_log_error_bad_request()
        {
            //Arrange
            var logError = new CreateLogErrorViewModel(
                userId: 1,
                title: "fakeTitle",
                details: "fakeDetails",
                source: "localhost",
                level: ELevel.Debug,
                environment: EEnvironment.Development);
            _logErrorServiceMock.Setup(x => x.CreateAsync(It.IsAny<CreateLogErrorViewModel>(), default))
                .Returns(Task.FromResult(false));

            //Act
            var orderController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await orderController.CreateLogErrorAsync(logError) as BadRequestResult;

            //Assert
            actionResult.StatusCode.Should()
                .Be((int)System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        [Trait("POST - Operation", "Create")]
        public async Task Create_log_error_with_invalid_view_model_bad_request()
        {
            //Arrange
            var logError = new CreateLogErrorViewModel(
                userId: 1,
                title: null,
                details: "fakeDetails",
                source: null,
                level: ELevel.Debug,
                environment: EEnvironment.Development);

            //Act
            var orderController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await orderController.CreateLogErrorAsync(logError) as BadRequestResult;

            //Assert
            actionResult.StatusCode.Should()
                .Be((int)System.Net.HttpStatusCode.BadRequest);
        }
    }
}
