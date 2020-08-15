using ErrorCentral.Application.Services;
using ErrorCentral.Application.Settings;
using ErrorCentral.Application.ViewModels.User;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using ErrorCentral.Domain.SeedWork;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ErrorCentral.UnitTests.Application
{
    public class UserServiceTest
    {
        private const string Email = "user@user.com";
        private const string FirstName = "User";
        private const string LastName = "User";
        private const string Password = "sso2tLHp35Q!";

        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITokenService> _tokenService;

        public UserServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tokenService = new Mock<ITokenService>();
        }

        [Fact(DisplayName = "Create - Return Error If User Already Exists")]
        [Trait("Operation", "Create")]
        public async Task Should_fail_if_user_already_exists()
        {

            CreateUserViewModel user = new CreateUserViewModel
            {
                Email = UserServiceTest.Email,
                FirstName = UserServiceTest.FirstName,
                LastName = UserServiceTest.LastName,
                Password = UserServiceTest.Password
            };

            User rawUser = new User(email: Email, lastName: LastName, firstName: FirstName, password: Password);

            _userRepositoryMock.Setup(svc => svc.GetByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(rawUser));

            //Act
            UserService service = new UserService(_userRepositoryMock.Object, _tokenService.Object);
            Response<GetUserViewModel> result = await service.CreateAsync(user);

            Response<GetUserViewModel> expected = new Response<GetUserViewModel>(success: false, errors: new[] { "This user already exists." });

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Create - Return Error If User Can't be Registered")]
        [Trait("Operation", "Create")]
        public async Task Should_fail_if_user_cant_be_registered()
        {

            CreateUserViewModel user = new CreateUserViewModel
            {
                Email = UserServiceTest.Email,
                FirstName = UserServiceTest.FirstName,
                LastName = UserServiceTest.LastName,
                Password = UserServiceTest.Password
            };

            User rawUser = new User(email: Email, lastName: LastName, firstName: FirstName, password: Password);

            _userRepositoryMock.Setup(svc => svc.GetByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));

            _userRepositoryMock.Setup(svc => svc.UnitOfWork.SaveChangesAsync(default))
                .Returns(Task.FromResult(0));

            //Act
            UserService service = new UserService(_userRepositoryMock.Object, _tokenService.Object);
            Response<GetUserViewModel> result = await service.CreateAsync(user);

            Response<GetUserViewModel> expected = new Response<GetUserViewModel>(errors: new[] { "Unable to create user, try again later." }, success: false);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Create - Return Error if viewmodel password is Invalid")]
        [Trait("Operation", "Create")]
        public async Task Should_fail_if_viewmodel_password_is_invalid()
        {
            CreateUserViewModel user = new CreateUserViewModel
            {
                Email = UserServiceTest.Email,
                FirstName = UserServiceTest.FirstName,
                LastName = UserServiceTest.LastName,
                Password = "Password!"
            };

            User rawUser = new User(email: Email, lastName: LastName, firstName: FirstName, password: Password);

            _userRepositoryMock.Setup(svc => svc.GetByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));

            Response<GetUserViewModel> expected = new Response<GetUserViewModel>(
                success: false,
                errors: new[] { "Password length must have more than 8 characters, with at least one digit, one uppercase character, one lowercase character and one special character" }
            );

            //Act
            UserService service = new UserService(_userRepositoryMock.Object, _tokenService.Object);
            Response<GetUserViewModel> result = await service.CreateAsync(user);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Create - Return Error if viewmodel email is Invalid")]
        [Trait("Operation", "Create")]
        public async Task Should_fail_if_viewmodel_email_is_invalid()
        {
            CreateUserViewModel user = new CreateUserViewModel
            {
                Email = "email",
                FirstName = UserServiceTest.FirstName,
                LastName = UserServiceTest.LastName,
                Password = UserServiceTest.Password
            };

            User rawUser = new User(email: Email, lastName: LastName, firstName: FirstName, password: Password);

            _userRepositoryMock.Setup(svc => svc.GetByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));

            Response<GetUserViewModel> expected = new Response<GetUserViewModel>(errors: new[] { "Should be an email address!" }, success: false);

            //Act
            UserService service = new UserService(_userRepositoryMock.Object, _tokenService.Object);
            Response<GetUserViewModel> result = await service.CreateAsync(user);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Create - Return Error if viewmodel firstname is Invalid")]
        [Trait("Operation", "Create")]
        public async Task Should_fail_if_viewmodel_firstname_is_invalid()
        {
            CreateUserViewModel user = new CreateUserViewModel
            {
                Email = UserServiceTest.Email,
                FirstName = "First Name",
                LastName = UserServiceTest.LastName,
                Password = UserServiceTest.Password
            };

            User rawUser = new User(email: Email, lastName: LastName, firstName: FirstName, password: Password);

            _userRepositoryMock.Setup(svc => svc.GetByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));

            Response<GetUserViewModel> expected = new Response<GetUserViewModel>(
                success: false,
                errors: new[] { "First Name shouldn't contain whitespaces" }
            );

            //Act
            UserService service = new UserService(_userRepositoryMock.Object, _tokenService.Object);
            Response<GetUserViewModel> result = await service.CreateAsync(user);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Create - Return Error if viewmodel lastname is Invalid")]
        [Trait("Operation", "Create")]
        public async Task Should_fail_if_viewmodel_lastname_is_invalid()
        {
            CreateUserViewModel user = new CreateUserViewModel
            {
                Email = UserServiceTest.Email,
                FirstName = UserServiceTest.FirstName,
                LastName = "Last Name",
                Password = UserServiceTest.Password
            };

            User rawUser = new User(email: Email, lastName: LastName, firstName: FirstName, password: Password);

            _userRepositoryMock.Setup(svc => svc.GetByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));

            Response<GetUserViewModel> expected = new Response<GetUserViewModel>(success: false, errors: new[] { "Last Name shouldn't contain whitespaces" });

            //Act
            UserService service = new UserService(_userRepositoryMock.Object, _tokenService.Object);
            Response<GetUserViewModel> result = await service.CreateAsync(user);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }


        [Fact(DisplayName = "Create - Return User if User Was Registered")]
        [Trait("Operation", "Create")]
        public async Task Should_register_user()
        {
            CreateUserViewModel user = new CreateUserViewModel
            {
                Email = UserServiceTest.Email,
                FirstName = UserServiceTest.FirstName,
                LastName = UserServiceTest.LastName,
                Password = UserServiceTest.Password
            };

            User rawUser = new User(email: Email, lastName: LastName, firstName: FirstName, password: Password);

            GetUserViewModel viewModel = new GetUserViewModel
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Id = 0,
                Token = "token",
            };

            Response<GetUserViewModel> expected = new Response<GetUserViewModel>(data: viewModel, success: true, errors: null);

            _userRepositoryMock.Setup(svc => svc.GetByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));

            _userRepositoryMock.Setup(svc => svc.UnitOfWork.SaveChangesAsync(default(CancellationToken)))
                .Returns(Task.FromResult(1));

            _userRepositoryMock.Setup(svc => svc.Create(It.IsAny<User>()))
                .Returns(rawUser);
            _tokenService.Setup(t => t.GenerateToken(rawUser))
                .Returns("token client");

            //Act
            UserService service = new UserService(_userRepositoryMock.Object, _tokenService.Object);
            Response<GetUserViewModel> result = await service.CreateAsync(user);

            //Assert
            result.Data.Email.Should().BeEquivalentTo(expected.Data.Email);
            result.Data.FirstName.Should().BeEquivalentTo(expected.Data.FirstName);
            result.Data.LastName.Should().BeEquivalentTo(expected.Data.LastName);
            result.Data.Id.Should().Be(expected.Data.Id);
            result.Errors.Should().BeEquivalentTo(expected.Errors);
            result.Success.Should().Be(expected.Success);
        }


        [Fact(DisplayName = "Authenticate - Can't authenticate if user does not exist")]
        [Trait("Operation", "Authenticate")]
        public async Task Should_fail_auth_if_user_doesnt_exist()
        {

            AuthenticateUserViewModel user = new AuthenticateUserViewModel
            {
                Email = Email,
                Password = Password
            };

            _userRepositoryMock.Setup(svc => svc.GetByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));

            Response<GetUserViewModel> expected = new Response<GetUserViewModel>
            (
                errors: new[] { "This user doesn't exist. " },
                success: false
            );

            //Act
            UserService service = new UserService(_userRepositoryMock.Object, _tokenService.Object);
            Response<GetUserViewModel> result = await service.AuthenticateAsync(user);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact(DisplayName = "Authenticate - Can't authenticate if email is wrong format")]
        [Trait("Operation", "Authenticate")]
        public async Task Should_fail_if_email_is_in_wrong_format()
        {

            AuthenticateUserViewModel user = new AuthenticateUserViewModel
            {
                Email = "email",
                Password = Password
            };

            _userRepositoryMock.Setup(svc => svc.GetByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));

            Response<GetUserViewModel> expected = new Response<GetUserViewModel>
            (
                errors: new[] { "Should be an email address!" },
                success: false
            );

            //Act
            UserService service = new UserService(_userRepositoryMock.Object, _tokenService.Object);
            Response<GetUserViewModel> result = await service.AuthenticateAsync(user);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }


        [Fact(DisplayName = "Authenticate - Can't authenticate if wrong password")]
        [Trait("Operation", "Authenticate")]
        public async Task Should_fail_auth_if_user_password_is_wrong()
        {

            AuthenticateUserViewModel user = new AuthenticateUserViewModel
            {
                Email = Email,
                Password = Password
            };

            User rawUser = new User(email: Email,
                lastName: LastName,
                firstName: FirstName,
                password: "otherPassword");

            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            rawUser.Password = passwordHasher.HashPassword(rawUser, rawUser.Password);

            _userRepositoryMock.Setup(svc => svc.GetByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(rawUser));

            Response<GetUserViewModel> expected = new Response<GetUserViewModel>(errors: new[] { "Wrong user/password combination, friend. " }, success: false);

            //Act
            UserService service = new UserService(_userRepositoryMock.Object, _tokenService.Object);
            Response<GetUserViewModel> result = await service.AuthenticateAsync(user);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }


        [Fact(DisplayName = "Authenticate - Can authenticate")]
        [Trait("Operation", "Authenticate")]
        public async Task Should_auth()
        {
            AuthenticateUserViewModel user = new AuthenticateUserViewModel
            {
                Email = Email,
                Password = Password
            };

            User rawUser = new User(email: Email,
                lastName: LastName,
                firstName: FirstName,
                password: Password);

            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            rawUser.Password = passwordHasher.HashPassword(rawUser, rawUser.Password);

            _userRepositoryMock.Setup(svc => svc.GetByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(rawUser));
            _tokenService.Setup(t => t.GenerateToken(rawUser))
                .Returns("token client");

            GetUserViewModel viewModel = new GetUserViewModel
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Id = 0
            };

            Response<GetUserViewModel> expected = new Response<GetUserViewModel>(errors: null, success: true, data: viewModel);

            //Act
            UserService service = new UserService(_userRepositoryMock.Object, _tokenService.Object);
            Response<GetUserViewModel> result = await service.AuthenticateAsync(user);

            //Assert
            result.Data.Email.Should().BeEquivalentTo(expected.Data.Email);
            result.Data.FirstName.Should().BeEquivalentTo(expected.Data.FirstName);
            result.Data.LastName.Should().BeEquivalentTo(expected.Data.LastName);
            result.Data.Id.Should().Be(expected.Data.Id);
            result.Errors.Should().BeEquivalentTo(expected.Errors);
            result.Success.Should().Be(expected.Success);
        }
    }
}
