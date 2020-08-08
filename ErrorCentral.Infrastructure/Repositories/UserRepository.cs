using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using ErrorCentral.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorCentral.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ErrorCentralContext _context; 
        public IUnitOfWork UnitOfWork => _context;

        public UserRepository(ErrorCentralContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> GetAsync(int userId)
        {
            return await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
        }

        public User Create(User user)
        {
            return _context.Add(user).Entity;
        }
    }
}
