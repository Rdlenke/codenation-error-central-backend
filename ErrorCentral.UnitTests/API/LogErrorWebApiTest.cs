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
using ErrorCentral.Domain.SeedWork;

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
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.CreateLogErrorAsync(logError) as OkResult;

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
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.CreateLogErrorAsync(logError) as BadRequestResult;

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
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.CreateLogErrorAsync(logError) as BadRequestResult;

            //Assert
            actionResult.StatusCode.Should()
                .Be((int)System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [Trait("GET - Operation", "Detail")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Get_log_error_unsuccessfully(int id)
        {
            //Arrange 
            var response = new Response<LogErrorDetailsViewModel>(
                success: false,
                errors: new [] { $"There isn't an log with id {id}" }
            );

            _logErrorServiceMock.Setup(x => x.GetLogError(id)).Returns(
                Task.FromResult(response));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.GetLogError(id);
            var result = actionResult.Result as NotFoundObjectResult;

            //Asserts
            result.StatusCode.Should().Be((int)System.Net.HttpStatusCode.NotFound);
        }

        [Theory]
        [Trait("Delete - Operation", "Remove")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Remove_log_error_bad_request(int id)
        {
            //Arrange
            _logErrorServiceMock.Setup(x => x.RemoveAsync(id))
                .Returns(Task.FromResult(false));
            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.RemoveLogErrorAsync(id) as BadRequestResult;

            //Assert
            actionResult.StatusCode.Should()
                .Be((int)System.Net.HttpStatusCode.BadRequest);
        }
    }
}
