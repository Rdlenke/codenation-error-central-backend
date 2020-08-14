using ErrorCentral.Application.ViewModels.User;
using ErrorCentral.Application.ViewModels.Validators;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErrorCentral.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
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

            User createdUser = _userRepository.Create(newUser);

            bool created = await _userRepository.UnitOfWork.SaveChangesAsync() > 0;

            if (!created)
            {
                return new GetUserViewModel
                {
                    Errors = new[] { "Unable to create user, try again later." }
                };
            }

            string token = _tokenService.GenerateToken(newUser);

            return new GetUserViewModel
            {
                Id = createdUser.Id,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Token = token,
                Email = createdUser.Email,
                Success = true,
                Guid = createdUser.Guid
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

            string token = _tokenService.GenerateToken(user);

            return new GetUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token,
                Success = true,
                Email = user.Email,
                Guid = user.Guid
            };
        }
    }
}