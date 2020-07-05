using ErrorCentral.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErrorCentral.Services
{
    public interface IUserService
    {
        public Task<GetUserViewModel> CreateAsync(CreateUserViewModel model);
        public Task<GetUserViewModel> AuthenticateAsync(AuthenticateUserViewModel model);
    }
}