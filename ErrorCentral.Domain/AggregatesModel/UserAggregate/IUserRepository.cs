using ErrorCentral.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ErrorCentral.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetAsync(int userId);
        Task<IEnumerable<User>> GetAsync();
        Task<User> GetByEmailAsync(string email);
        public User Create(User user);
    }
}
