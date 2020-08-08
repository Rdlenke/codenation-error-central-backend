using FluentAssertions;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ErrorCentral.IntegrationTests
{
    public class AuthenticationTest : IClassFixture<ApiTestFixture>
    {
        public HttpClient Client { get; }
        public AuthenticationTest(ApiTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        [Theory]
        [InlineData("/api/v1/logerrors/1")]
        public async Task ReturnsNotAuthorizedWithoutTheTokenInRequestGet(string url)
        {
            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("/api/v1/logerrors")]
        public async Task ReturnsNotAuthorizedWithoutTheTokenInRequestGetAll(string url)
        {
            // Act
            var response = await Client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("/api/v1/logerrors/1")]
        public async Task ReturnsNotAuthorizedWithoutTheTokenInRequestDelete(string url)
        {
            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("/api/v1/logerrors")]
        public async Task ReturnsNotAuthorizedWithoutTheTokenInRequestPost(string url)
        {
            // Act
            var response = await Client.PostAsync(url, It.IsAny<StringContent>());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
