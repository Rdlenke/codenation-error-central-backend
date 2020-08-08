using ErrorCentral.Application.ViewModels.User;
using ErrorCentral.UnitTests.Builders.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ErrorCentral.IntegrationTests
{
    public class UsersTest : IClassFixture<ApiTestFixture>
    {
        JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private const string baseUrl = "/api/v1/user";
        public HttpClient Client { get; }
        public UsersTest(ApiTestFixture factory)
        {
            Client = factory.CreateClient();
        }


        [Fact]
        public async Task ReturnsSucessWhenRegistering()
        {
            // Arrange
            var jsonContent = new StringContent(JsonSerializer.Serialize(new CreateUserViewModelBuilder().Build()), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync($"{baseUrl}/CreateUser", jsonContent);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ReturnsBadRequestWhenRegistering()
        {
            // Arrange
            var jsonContent = new StringContent(JsonSerializer.Serialize(new CreateUserViewModelBuilder().Build()), Encoding.UTF8, "application/json");
            await Client.PostAsync($"{baseUrl}/CreateUser", jsonContent);

            // Act
            var response = await Client.PostAsync($"{baseUrl}/CreateUser", jsonContent);

            // Assert
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        }

        [Fact]
        public async Task ReturnsSucessAuthenticating()
        {
            // Arrange
            var jsonContent = new StringContent(JsonSerializer.Serialize(new CreateUserViewModelBuilder().Build()), Encoding.UTF8, "application/json");
            await Client.PostAsync($"{baseUrl}/CreateUser", jsonContent);

            // Act
            jsonContent = new StringContent(JsonSerializer.Serialize(new AuthenticateUserViewModelBuilder().Build()), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"{baseUrl}/AuthenticateUser", jsonContent);

            // Assert
            response.StatusCode = System.Net.HttpStatusCode.OK;
        }

        [Fact]
        public async Task ReturnsFailureAuthenticating()
        {
            // Arrange
            var jsonContent = new StringContent(JsonSerializer.Serialize(new CreateUserViewModelBuilder().Build()), Encoding.UTF8, "application/json");
            await Client.PostAsync($"{baseUrl}/CreateUser", jsonContent);

            // Act
            AuthenticateUserViewModel authenticateUserViewModel = new AuthenticateUserViewModelBuilder().Build();
            authenticateUserViewModel.Password = "1";


            jsonContent = new StringContent(JsonSerializer.Serialize(authenticateUserViewModel), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"{baseUrl}/AuthenticateUser", jsonContent);

            // Assert
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        }
    }
}
