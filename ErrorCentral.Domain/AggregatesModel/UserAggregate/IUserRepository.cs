using System.Threading.Tasks;

namespace ErrorCentral.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository
    {
        Task<User> GetAsync(int userId);
    }
}
