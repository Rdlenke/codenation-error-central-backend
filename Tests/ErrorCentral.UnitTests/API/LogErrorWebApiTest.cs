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
using System;
using System.Net;

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
        public void Return_exception_to_initial_controller_without_service()
        {
            // Act
            Action act = () => new LogErrorsController(null, _loggerMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'logErrorService')");
        }

        [Fact]
        public void Return_exception_to_initial_controller_without_logger()
        {
            // Act
            Action act = () => new LogErrorsController(_logErrorServiceMock.Object, null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'logger')");
        }

        [Fact]
        [Trait("POST - Operation", "Create")]
        public async Task Create_log_error_success()
        {
            //Arrange
            var logError = new CreateLogErrorViewModel()
            {
                Title = "Run-time exception (line 8): Attempted to divide by zero.",
                Details = "[System.DivideByZeroException: Attempted to divide by zero.] \nat Program.Main() :line 8",
                Source = "http://production.com/",
                Level = ELevel.Error,
                Environment = EEnvironment.Production,
                UserId = 1
            };

            _logErrorServiceMock.Setup(x => x.CreateAsync(It.IsAny<CreateLogErrorViewModel>(), default))
                .Returns(Task.FromResult(true));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.PostAsync(logError) as OkResult;

            //Assert
            actionResult.StatusCode.Should()
                .Be((int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        [Trait("POST - Operation", "Create")]
        public async Task Create_log_error_bad_request()
        {
            //Arrange
            var logError = new CreateLogErrorViewModel()
            {
                Title = "Run-time exception (line 8): Attempted to divide by zero.",
                Details = "[System.DivideByZeroException: Attempted to divide by zero.] \nat Program.Main() :line 8",
                Source = "http://production.com/",
                Level = ELevel.Error,
                Environment = EEnvironment.Production,
                UserId = 1
            };
            _logErrorServiceMock.Setup(x => x.CreateAsync(It.IsAny<CreateLogErrorViewModel>(), default))
                .Returns(Task.FromResult(false));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.PostAsync(logError) as BadRequestResult;

            //Assert
            actionResult.StatusCode.Should()
                .Be((int)System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        [Trait("POST - Operation", "Create")]
        public async Task Create_log_error_with_invalid_view_model_bad_request()
        {
            //Arrange
            var logError = new CreateLogErrorViewModel()
            {
                Title = "Run-time exception (line 8): Attempted to divide by zero.",
                Details = "[System.DivideByZeroException: Attempted to divide by zero.] \nat Program.Main() :line 8",
                Source = "http://production.com/",
                Level = ELevel.Error,
                Environment = EEnvironment.Production,
                UserId = 1
            };

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.PostAsync(logError) as BadRequestResult;

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
            var actionResult = await logErrorController.GetAsync(id);
            var result = actionResult.Result as NotFoundObjectResult;

            //Asserts
            result.StatusCode.Should().Be((int)System.Net.HttpStatusCode.NotFound);
        }

        [Theory]
        [Trait("Delete - Operation", "Delete")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Delete_log_error_bad_request(int id)
        {
            //Arrange
            var response = new Response<int>(false, new[] { $"object with id {id} not found" });
            _logErrorServiceMock.Setup(x => x.RemoveAsync(id))
                .Returns(Task.FromResult(response));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.DeleteAsync(id);


            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            badRequestResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var result = Assert.IsType<Response<int>>(badRequestResult.Value);
            result.Success.Should().BeFalse();
            result.Errors.Length.Should().Be(1);
            result.Errors.Should().Equal(response.Errors);
        }

        [Theory]
        [Trait("Delete - Operation", "Delete")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Delete_log_error_sucess(int id)
        {
            //Arrange
            var response = new Response<int>(id, true);
            _logErrorServiceMock.Setup(x => x.RemoveAsync(id))
                .Returns(Task.FromResult(response));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.DeleteAsync(id);


            //Assert
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            okRequestResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var result = Assert.IsType<Response<int>>(okRequestResult.Value);
            result.Success.Should().BeTrue();
            result.Errors.Should().BeNull();
        }
    }
}
