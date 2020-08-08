using ErrorCentral.Application.Settings;
using ErrorCentral.Application.ViewModels.User;
using ErrorCentral.Application.ViewModels.Validators;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ErrorCentral.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly Jwt _jwt;

        public UserService(IUserRepository userRepository, Jwt jwt)
        {
            _userRepository = userRepository;
            _jwt = jwt;
        }

        public async Task<GetUserViewModel> CreateAsync(CreateUserViewModel model)
        {
            CreateUserViewModelValidation validator = new CreateUserViewModelValidation();
            ValidationResult result = validator.Validate(model);

            if (!result.IsValid)
            {
                return new GetUserViewModel
                {
                    Errors = result.Errors.Select(x => x.ErrorMessage).ToArray()
                };
            }

            User user = await _userRepository.GetByEmailAsync(model.Email);

            if (user != null)
            {
                return new GetUserViewModel
                {
                    Errors = new[] { "This user already exists." }
                };
            }

            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            string hashed = passwordHasher.HashPassword(user, model.Password);

            User newUser = new User(
                email: model.Email, 
                lastName: model.LastName, 
                firstName: model.FirstName,
                password: hashed);

            _userRepository.Create(newUser);

            bool created = await _userRepository.UnitOfWork.SaveChangesAsync() > 0;

            if (!created)
            {
                return new GetUserViewModel
                {
                    Errors = new[] { "Unable to create user, try again later." }
                };
            }

            string token = GenerateToken(newUser);

            return new GetUserViewModel
            {
                Id = newUser.Id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Token = token,
                Email = newUser.Email,
                Success = true,
            };
        }

        public async Task<GetUserViewModel> AuthenticateAsync(AuthenticateUserViewModel model)
        {
            AuthenticateUserViewModelValidator validator = new AuthenticateUserViewModelValidator();

            ValidationResult result = validator.Validate(model);

            if (!result.IsValid)
            {
                return new GetUserViewModel
                {
                    Success = false,
                    Errors = result.Errors.Select(x => x.ErrorMessage).ToArray()
                };
            }

            User user = await _userRepository.GetByEmailAsync(model.Email);

            if (user == null)
            {
                return new GetUserViewModel
                {
                    Success = false,
                    Errors = new[] { "This user doesn't exist. " }
                };
            }

            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            PasswordVerificationResult pvr = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (pvr == PasswordVerificationResult.Failed)
            {
                return new GetUserViewModel
                {
                    Success = false,
                    Errors = new[] { "Wrong user/password combination, friend. " }
                };
            }

            string token = GenerateToken(user);

            return new GetUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token,
                Success = true,
                Email = user.Email
            };
        }

        private string GenerateToken(User user)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_jwt.Secret);

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