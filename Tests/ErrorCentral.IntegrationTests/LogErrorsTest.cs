using ErrorCentral.Application.ViewModels.LogError;
using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ErrorCentral.IntegrationTests
{
    public class LogErrorsTest : IClassFixture<ApiTestFixture>
    {
        JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private const string baseUrl = "/api/v1/logerrors";
        public HttpClient Client { get; }
        public LogErrorsTest(ApiTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        #region Sem Token
        //[Theory]
        //[InlineData(1)]
        //[InlineData(2)]
        //[InlineData(3)]
        //[InlineData(4)]
        //public async Task ReturnsNotAuthorizedWithoutTheTokenInRequestGet(int id)
        //{
        //    // Act
        //    var response = await Client.GetAsync($"{baseUrl}/{id}");

        //    // Assert
        //    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        //}

        //[Fact]
        //public async Task ReturnsNotAuthorizedWithoutTheTokenInRequestGetAll()
        //{
        //    // Act
        //    var response = await Client.GetAsync($"{baseUrl}");

        //    // Assert
        //    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        //}

        //[Theory]
        //[InlineData(1)]
        //[InlineData(2)]
        //[InlineData(3)]
        //[InlineData(4)]
        //public async Task ReturnsNotAuthorizedWithoutTheTokenInRequestDelete(int id)
        //{
        //    // Act
        //    var response = await Client.DeleteAsync($"{baseUrl}/{id}");

        //    // Assert
        //    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        //}

        //[Fact]
        //public async Task ReturnsNotAuthorizedWithoutTheTokenInRequestPost()
        //{
        //    // Arrange
        //    var jsonContent = GetValidNewLogErrorJson();

        //    // Act
        //    var response = await Client.PostAsync($"{baseUrl}", jsonContent);

        //    // Assert
        //    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        //}
        #endregion



        #region Mock
        private StringContent GetValidNewLogErrorJson()
        {
            var request = new CreateLogErrorViewModel()
            {
                Title = "Run-time exception (line 8): Attempted to divide by zero.",
                Details = "[System.DivideByZeroException: Attempted to divide by zero.] \nat Program.Main() :line 8",
                Source = "http://production.com/",
                Level = ELevel.Error,
                Environment = EEnvironment.Production,
                UserId = 1
            };
            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            return jsonContent;
        }
        #endregion
    }
}
