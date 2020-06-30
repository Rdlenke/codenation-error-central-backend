using ErrorCentral.Contracts;
using ErrorCentral.Data;
using ErrorCentral.Models;
using ErrorCentral.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ErrorCentral.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly AuthenticationOptions _authenticationOptions;

        public UserService(UserManager<User> userManager, AuthenticationOptions authenticationOptions)
        {
            _userManager = userManager;
            _authenticationOptions = authenticationOptions;
        }

        public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
        {
            User user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                return new AuthenticationResponse
                {
                    Sucess = false,
                    Errors = new[] { "Já existe um usuário com este email." }
                };
            }

            User newUser = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email
            };

            IdentityResult created = await _userManager.CreateAsync(newUser, request.Password);

            if(!created.Succeeded)
            {
                return new AuthenticationResponse
                {
                    Sucess = false,
                    Errors = created.Errors.Select(x => x.Description).ToArray<string>()
                };
            }

            string token = GenerateToken(newUser);

            return new AuthenticationResponse
            {
                Id = newUser.Id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Token = token,
                Sucess = true,
            };
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            User user = await _userManager.FindByEmailAsync(request.Email);

            if(user == null)
            {
                return null;
            }

            string token = GenerateToken(user);

            return new AuthenticationResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token,
                Sucess = true,
            };
        }

        private string GenerateToken(User user)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_authenticationOptions.Secret);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}
