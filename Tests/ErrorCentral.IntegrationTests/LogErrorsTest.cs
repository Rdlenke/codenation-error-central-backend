using ErrorCentral.Application.Services;
using ErrorCentral.Application.Settings;
using ErrorCentral.UnitTests.Builders.ViewModels;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        [Fact]
        public async Task ReturnsSuccessGiverValidCreateLogError()
        {
            // Arrange
            var token = ApiTokenHelper.GetNormalUserToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonContent = new StringContent(JsonSerializer.Serialize(new CreateLogErrorViewModelBuilder().Build()), Encoding.UTF8, "application/json");
            
            // Act
            var response = await Client.PostAsync($"{baseUrl}", jsonContent);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
