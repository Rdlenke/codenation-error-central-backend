using ErrorCentral.Contracts;
using ErrorCentral.Data;
using ErrorCentral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErrorCentral.Services
{
    public interface IUserService
    {

        public Task<AuthenticationResponse> RegisterAsync(RegisterRequest request);
        public Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
    }
}
