using ErrorCentral.Application.ViewModels.User;
using ErrorCentral.Application.ViewModels.Validators;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using ErrorCentral.Domain.SeedWork;
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

        public async Task<Response<GetUserViewModel>> CreateAsync(CreateUserViewModel model)
        {
            CreateUserViewModelValidation validator = new CreateUserViewModelValidation();
            ValidationResult result = validator.Validate(model);

            if (!result.IsValid)
            {
                Response<GetUserViewModel> response = new Response<GetUserViewModel>(success: false, errors: result.Errors.Select(x => x.ErrorMessage).ToArray());

                return response;
            }

            User user = await _userRepository.GetByEmailAsync(model.Email);

            if (user != null)
            {
                Response<GetUserViewModel> response = new Response<GetUserViewModel>(success: false, errors: new[] { "This user already exists." });

                return response;
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
                Response<GetUserViewModel> response = new Response<GetUserViewModel>(success: false, errors: new[] { "Unable to create user, try again later." });

                return response;
            }

            User createdUser = await _userRepository.GetByEmailAsync(newUser.Email);

            string token = _tokenService.GenerateToken(createdUser);

            GetUserViewModel responseViewModel = new GetUserViewModel
                {
                    Id = createdUser.Id,
                    FirstName = createdUser.FirstName,
                    LastName = createdUser.LastName,
                    Token = token,
                    Email = createdUser.Email,
                    Guid = createdUser.Guid
            };

            return new Response<GetUserViewModel>(success: true, errors: null, data: responseViewModel);
        }

        public async Task<Response<GetUserViewModel>> AuthenticateAsync(AuthenticateUserViewModel model)
        {
            AuthenticateUserViewModelValidator validator = new AuthenticateUserViewModelValidator();

            ValidationResult result = validator.Validate(model);

            if (!result.IsValid)
            {
                Response<GetUserViewModel> response = new Response<GetUserViewModel>(success: false, errors: result.Errors.Select(x => x.ErrorMessage).ToArray());

                return response;
            }

            User user = await _userRepository.GetByEmailAsync(model.Email);

            if (user == null)
            {
                Response<GetUserViewModel> response = new Response<GetUserViewModel>(success: false, errors: new[] { "This user doesn't exist. " });

                return response;
            }

            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            PasswordVerificationResult pvr = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (pvr == PasswordVerificationResult.Failed)
            {
                Response<GetUserViewModel> response = new Response<GetUserViewModel>(success: false, errors: new[] { "Wrong user/password combination, friend. " });


                return response;
            }

            string token = _tokenService.GenerateToken(user);

            GetUserViewModel responseUserViewModel = new GetUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token,
                Email = user.Email,
                Guid = user.Guid
            };

            return new Response<GetUserViewModel>(success: true, data: responseUserViewModel, errors: null);
        }
    }
}
