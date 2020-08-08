using ErrorCentral.Application.ViewModels.User;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ErrorCentral.Application.Services
{
    public interface IUserService
    {
        public Task<GetUserViewModel> CreateAsync(CreateUserViewModel model);
        public Task<GetUserViewModel> AuthenticateAsync(AuthenticateUserViewModel model);
    }
}