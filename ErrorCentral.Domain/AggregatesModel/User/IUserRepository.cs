using System.Threading.Tasks;

namespace ErrorCentral.Domain.AggregatesModel.User
{
    public interface IUserRepository
    {
        Task<User> GetAsync(int userId);
    }
}
