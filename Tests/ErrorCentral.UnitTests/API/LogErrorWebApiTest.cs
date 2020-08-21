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
using System.Collections.Generic;
using System.Net;
using ErrorCentral.Application.ViewModels.Misc;
using System;
using ErrorCentral.UnitTests.Builders.ViewModels;

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
            var logError = new CreateLogErrorViewModelBuilder().Build();
            var expected = new Response<CreateLogErrorViewModel>(logError, true);

            _logErrorServiceMock.Setup(x => x.CreateAsync(It.IsAny<CreateLogErrorViewModel>(), default))
                .Returns(Task.FromResult(expected));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.PostAsync(logError);

            //Assert
            var createdRequestResult = Assert.IsType<CreatedResult>(actionResult.Result);
            createdRequestResult.StatusCode.Should().Be((int)HttpStatusCode.Created);
            var result = Assert.IsType<Response<CreateLogErrorViewModel>>(createdRequestResult.Value);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        [Trait("POST - Operation", "Create")]
        public async Task Create_log_error_bad_request_with_userId()
        {
            //Arrange
            var logError = new CreateLogErrorViewModelBuilder()
                .WithUserId(0)
                .Build();
            var expected = new Response<CreateLogErrorViewModel>(
                data: logError,
                false,
                errors: new[] { "UserId must be greater than 0" }
            );
            _logErrorServiceMock.Setup(x => x.CreateAsync(logError, default))
                .Returns(Task.FromResult(expected));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.PostAsync(logError);

            ////Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            badRequestResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var result = Assert.IsType<Response<CreateLogErrorViewModel>>(badRequestResult.Value);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        [Trait("POST - Operation", "Create")]
        public async Task Create_log_error_with_invalid_view_model_bad_request()
        {
            //Arrange
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
            _logErrorServiceMock.Setup(x => x.CreateAsync(logError, default))
                .Returns(Task.FromResult(expected));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.PostAsync(logError);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            badRequestResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var result = Assert.IsType<Response<CreateLogErrorViewModel>>(badRequestResult.Value);
            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [Trait("GET - Operation", "Detail")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Get_log_error_with_id_invald_and_returns_not_found(int id)
        {
            //Arrange
            var expected = new Response<LogErrorDetailsViewModel>(
                success: false,
                errors: new [] { $"There isn't an log with id {id}" }
            );

            _logErrorServiceMock.Setup(x => x.GetLogError(id)).Returns(
                Task.FromResult(expected));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.GetAsync(id);

            //Asserts
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            notFoundObjectResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            var result = Assert.IsType<Response<LogErrorDetailsViewModel>>(notFoundObjectResult.Value);
            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [Trait("GET - Operation", "Detail")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Get_log_error_with_id_valid(int id)
        {
            //Arrange
            var expected = new Response<LogErrorDetailsViewModel>(
                data: new LogErrorDetailsViewModelBuilder().Build(),
                success: true
            );

            _logErrorServiceMock.Setup(x => x.GetLogError(id)).Returns(
                Task.FromResult(expected));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.GetAsync(id);

            //Asserts
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            okObjectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var result = Assert.IsType<Response<LogErrorDetailsViewModel>>(okObjectResult.Value);
            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(1, "My LogError", EEnvironment.Development, ELevel.Debug, "Source", "Details", 100, false)]
        [InlineData(2, "Your Log Error", EEnvironment.Homologation, ELevel.Error, "Source", "Details", 300, false)]
        [InlineData(5, "Our Log Error", EEnvironment.Production, ELevel.Warning, "Source", "Details", 300, false)]
        public async void Get_all_log_errors(int userId, string title, EEnvironment environment, ELevel level, string source, string details, int events, bool filed)
        {
            //Arrange
            List<ListLogErrorsViewModel> listLogErrorsViewModel = new List<ListLogErrorsViewModel>();

            listLogErrorsViewModel.Add(
                new ListLogErrorsViewModel(
                    userId: userId,
                    filed: filed,
                    title: title,
                    environment: environment,
                    level: level,
                    source: source,
                    details: details,
                    events: events
                ));

            Response<List<ListLogErrorsViewModel>> response = new Response<List<ListLogErrorsViewModel>>(data: listLogErrorsViewModel, success: true, errors: null);

            _logErrorServiceMock.Setup(x => x.Get(null))
                .Returns(Task.FromResult(response));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.GetAll();

            var result = actionResult.Result as OkObjectResult;


            // Assert
            result.StatusCode.Should()
                .Be((int)HttpStatusCode.OK);

            var obtainedResponse = result.Value as Response<List<ListLogErrorsViewModel>>;

            obtainedResponse.Should()
                .BeEquivalentTo(response);
        }

        [Fact]
        public async void Get_all_errors_by_environment()
        {
            //Arrange
            List<ListLogErrorsViewModel> listLogErrorsViewModel = new List<ListLogErrorsViewModel>();

            listLogErrorsViewModel.Add(CreateListLogErrorsViewModel(5, "Our Log Error", EEnvironment.Production, ELevel.Warning, "Source", "Details", 300, false));
            listLogErrorsViewModel.Add(CreateListLogErrorsViewModel(1, "Log Error", EEnvironment.Production, ELevel.Warning, "Source", "Details", 300, false));
            listLogErrorsViewModel.Add(CreateListLogErrorsViewModel(1, "Log Error", EEnvironment.Development, ELevel.Warning, "Source", "Details", 300, false));

            List<ListLogErrorsViewModel> expected = new List<ListLogErrorsViewModel>();
            listLogErrorsViewModel.Add(listLogErrorsViewModel[0]);
            listLogErrorsViewModel.Add(listLogErrorsViewModel[1]);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel { Environment = EEnvironment.Production };

            Response<List<ListLogErrorsViewModel>> response = new Response<List<ListLogErrorsViewModel>>(data: expected, success: true, errors: null);

            _logErrorServiceMock.Setup(x => x.Get(query))
                .Returns(Task.FromResult(response));

            // Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.GetAll(query);

            var result = actionResult.Result as OkObjectResult;


            // Assert
            result.StatusCode.Should()
                .Be((int)HttpStatusCode.OK);

            var obtainedResponse = result.Value as Response<List<ListLogErrorsViewModel>>;

            obtainedResponse.Should()
                .BeEquivalentTo(response);
        }

        [Fact]
        public async Task Get_all_archived_errors()
        {
            //Arrange
            List<ListLogErrorsViewModel> viewModels = new List<ListLogErrorsViewModel> { new ListLogerrorsViewModelBuilder().Build() };
            Response <List<ListLogErrorsViewModel>> response = new Response<List<ListLogErrorsViewModel>>(data: viewModels, success: true, errors: null);

            _logErrorServiceMock.Setup(x => x.GetArchived()).Returns(Task.FromResult(response));

            // Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.GetArchived();

            OkObjectResult result = actionResult.Result as OkObjectResult;

            // Assert
            result.StatusCode.Should()
                .Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_all_archived_errors_fail()
        {
            //Arrange
            var response = new Response<List<ListLogErrorsViewModel>>(success: false, errors: null);
            _logErrorServiceMock.Setup(x => x.GetArchived()).Returns(Task.FromResult(response));

            // Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.GetArchived();

            NotFoundObjectResult result = actionResult.Result as NotFoundObjectResult;

            //Assert
            result.StatusCode.Should()
                .Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async void Get_all_errors_fail()
        {
            //Arrange
            _logErrorServiceMock.Setup(x => x.Get(null))
                .Returns(Task.FromResult(new Response<List<ListLogErrorsViewModel>>(success: false, errors: null)));

            //Act

            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.GetAll();

            var result = actionResult.Result as NotFoundObjectResult;

            // Assert
            result.StatusCode.Should()
                .Be((int)HttpStatusCode.NotFound);
        }

        private static ListLogErrorsViewModel CreateListLogErrorsViewModel(int userId,
            string title,
            EEnvironment environment,
            ELevel level,
            string source,
            string details,
            int events,
            bool filed)
        {
            return new ListLogErrorsViewModel(
                    userId: userId,
                    title: title,
                    filed: filed,
                    environment: environment,
                    level: level,
                    source: source,
                    details: details,
                    events: events
                );
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
            var response = new Response<int>(id, false, new[] { $"object with id {id} not found" });
            _logErrorServiceMock.Setup(x => x.RemoveAsync(id))
                .Returns(Task.FromResult(response));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.DeleteAsync(id);


            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            badRequestResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var result = Assert.IsType<Response<int>>(badRequestResult.Value);
            result.Should()
                .BeEquivalentTo(response);
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
            result.Should()
                .BeEquivalentTo(response);
        }

        [Theory]
        [Trait("Patch - Operation", "Archive")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Archive_log_error_bad_request(int id)
        {
            //Arrange
            var response = new Response<int>(false, new[] { $"object with id {id} not found" });
            _logErrorServiceMock.Setup(x => x.ArchiveAsync(id))
                .Returns(Task.FromResult(response));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.ArchiveAsync(id);


            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            badRequestResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var result = Assert.IsType<Response<int>>(badRequestResult.Value);
            result.Success.Should().BeFalse();
            result.Errors.Length.Should().Be(1);
            result.Errors.Should().Equal(response.Errors);
        }

        [Theory]
        [Trait("Patch - Operation", "Archive")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Archive_log_error_sucess(int id)
        {
            //Arrange
            var response = new Response<int>(id, true);
            _logErrorServiceMock.Setup(x => x.ArchiveAsync(id))
                .Returns(Task.FromResult(response));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.ArchiveAsync(id);


            //Assert
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            okRequestResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var result = Assert.IsType<Response<int>>(okRequestResult.Value);
            result.Success.Should().BeTrue();
            result.Errors.Should().BeNull();
        }

        [Theory]
        [Trait("Patch - Operation", "Unarchive")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task Unarchive_log_error_success(int id)
        {
            //Arrange
            var response = new Response<int>(id, true);

            _logErrorServiceMock.Setup(x => x.UnarchiveAsync(id))
                .Returns(Task.FromResult(response));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.UnarchiveAsync(id);

            //Assert
            var okRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            okRequestResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var result = Assert.IsType<Response<int>>(okRequestResult.Value);
            result.Success.Should().BeTrue();
            result.Errors.Should().BeNull();
        }

        [Theory]
        [Trait("Patch - Operation", "Unarchive")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Unarchive_log_error_bad_request(int id)
        {
            //Arrange
            var response = new Response<int>(false, new[] { $"object with id {id} not found" });
            _logErrorServiceMock.Setup(x => x.UnarchiveAsync(id))
                .Returns(Task.FromResult(response));

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = await logErrorController.UnarchiveAsync(id);


            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            badRequestResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            var result = Assert.IsType<Response<int>>(badRequestResult.Value);
            result.Success.Should().BeFalse();
            result.Errors.Length.Should().Be(1);
            result.Errors.Should().Equal(response.Errors);
        }
    }
}
