using ErrorCentral.Application.ViewModels.User;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using ErrorCentral.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ErrorCentral.Application.Services
{
    public interface IUserService
    {
        public Task<Response<GetUserViewModel>> CreateAsync(CreateUserViewModel model);
        public Task<Response<GetUserViewModel>> AuthenticateAsync(AuthenticateUserViewModel model);
    }
}