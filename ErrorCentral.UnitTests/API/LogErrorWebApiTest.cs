﻿using ErrorCentral.API.v1.Controllers;
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
using FluentAssertions.Common;
using ErrorCentral.Application.ViewModels.Misc;

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

        [Theory]
        [InlineData(1, "My LogError", EEnvironment.Development, ELevel.Debug, "Source", "Details", 100)]
        [InlineData(2, "Your Log Error", EEnvironment.Homologation, ELevel.Error, "Source", "Details", 300)]
        [InlineData(5, "Our Log Error", EEnvironment.Production, ELevel.Warning, "Source", "Details", 300)]
        public void Get_all_log_errors(int userId, string title, EEnvironment environment, ELevel level, string source, string details, int events)
        {
            //Arrange
            List<ListLogErrorsViewModel> listLogErrorsViewModel = new List<ListLogErrorsViewModel>();

            listLogErrorsViewModel.Add(
                new ListLogErrorsViewModel(
                    userId: userId,
                    title: title,
                    environment: environment,
                    level: level,
                    source: source,
                    details: details,
                    events: events
                ));

            Response<List<ListLogErrorsViewModel>> response = new Response<List<ListLogErrorsViewModel>>(data: listLogErrorsViewModel, success: true, errors: null);

            _logErrorServiceMock.Setup(x => x.Get(null))
                .Returns(response);

            //Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = logErrorController.GetAll();

            var result = actionResult.Result as OkObjectResult;


            // Assert
            result.StatusCode.Should()
                .Be((int)HttpStatusCode.OK);

            var obtainedResponse = result.Value as Response<List<ListLogErrorsViewModel>>;

            obtainedResponse.Should()
                .BeEquivalentTo(response);
        }

        [Fact]
        public void Get_all_errors_by_environment()
        {
            //Arrange
            List<ListLogErrorsViewModel> listLogErrorsViewModel = new List<ListLogErrorsViewModel>();

            listLogErrorsViewModel.Add(CreateListLogErrorsViewModel(5, "Our Log Error", EEnvironment.Production, ELevel.Warning, "Source", "Details", 300));
            listLogErrorsViewModel.Add(CreateListLogErrorsViewModel(1, "Log Error", EEnvironment.Production, ELevel.Warning, "Source", "Details", 300));
            listLogErrorsViewModel.Add(CreateListLogErrorsViewModel(1, "Log Error", EEnvironment.Development, ELevel.Warning, "Source", "Details", 300));

            List<ListLogErrorsViewModel> expected = new List<ListLogErrorsViewModel>();
            listLogErrorsViewModel.Add(listLogErrorsViewModel[0]);
            listLogErrorsViewModel.Add(listLogErrorsViewModel[1]);

            GetLogErrorsQueryViewModel query = new GetLogErrorsQueryViewModel { Environment = EEnvironment.Production };

            Response<List<ListLogErrorsViewModel>> response = new Response<List<ListLogErrorsViewModel>>(data: expected, success: true, errors: null);

            _logErrorServiceMock.Setup(x => x.Get(query))
                .Returns(response);

            // Act
            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = logErrorController.GetAll(query);

            var result = actionResult.Result as OkObjectResult;


            // Assert
            result.StatusCode.Should()
                .Be((int)HttpStatusCode.OK);

            var obtainedResponse = result.Value as Response<List<ListLogErrorsViewModel>>;

            obtainedResponse.Should()
                .BeEquivalentTo(response);
        }

        [Fact]
        public void Get_all_errors_fail()
        {
            //Arrange
            _logErrorServiceMock.Setup(x => x.Get(null))
                .Returns(new Response<List<ListLogErrorsViewModel>>(success: false, errors: null));

            //Act

            var logErrorController = new LogErrorsController(_logErrorServiceMock.Object, _loggerMock.Object);
            var actionResult = logErrorController.GetAll();

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
            int events)
        {
            return new ListLogErrorsViewModel(
                    userId: userId,
                    title: title,
                    environment: environment,
                    level: level,
                    source: source,
                    details: details,
                    events: events
                );
        }
    }
}
