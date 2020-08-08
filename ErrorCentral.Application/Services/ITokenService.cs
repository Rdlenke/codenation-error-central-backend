using ErrorCentral.Domain.AggregatesModel.UserAggregate;

namespace ErrorCentral.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
