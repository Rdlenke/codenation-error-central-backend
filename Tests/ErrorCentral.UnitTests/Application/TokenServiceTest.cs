using ErrorCentral.Application.Services;
using ErrorCentral.UnitTests.Builders.AggregatesModel;
using ErrorCentral.UnitTests.Builders.Settings;
using FluentAssertions;
using Xunit;

namespace ErrorCentral.UnitTests.Application
{
    public class TokenServiceTest
    {
        [Fact]
        public void Create_handle_return_token()
        {
            // Act
            var service = new TokenService(new JwtBuilder().Build());
            var result = service.GenerateToken(new UserBuilder().Build());

            // Assert
            result.Should().NotBeNullOrEmpty();
        }
    }
}
