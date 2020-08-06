using ErrorCentral.API.v1.Controllers;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.ViewModels.User;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ErrorCentral.UnitTests.API
{
    public class UserWebApiTest
    {
        private const string Email = "user@user.com";
        private const string FirstName = "User";
        private const string LastName = "User";
        private const string Password = "Password";
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<UserController>> _loggerMock;

        public UserWebApiTest()
        {
            _loggerMock = new Mock<ILogger<UserController>>();
            _userServiceMock = new Mock<IUserService>();
        }

        [Fact]
        public void Return_exception_to_initial_controller_without_service()
        {
            // Act
            Action act = () => new UserController(null, _loggerMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'userService')");
        }

        [Fact]
        public void Return_exception_to_initial_controller_without_logger()
        {
            // Act
            Action act = () => new UserController(_userServiceMock.Object, null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'logger')");
        }

        [Fact]
        [Trait("POST - Operation", "Create")]
        public async Task Create_User_Success()
        {

            string email = UserWebApiTest.Email;
            string firstName = UserWebApiTest.FirstName;
            string lastName = UserWebApiTest.LastName;
            string password = Password;

            CreateUserViewModel user = new CreateUserViewModel
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Password = password
            };

            GetUserViewModel registeredUser = new GetUserViewModel
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Success = true,
                Id = 1,
                Token = "token",
                Errors = null
            };


            _userServiceMock.Setup(x => x.CreateAsync(It.IsAny<CreateUserViewModel>()))
                .Returns(Task.FromResult(registeredUser));

            //Act
            UserController userController = new UserController(_userServiceMock.Object, _loggerMock.Object);
            OkObjectResult result = await userController.CreateUserAsync(user) as OkObjectResult;


            //Assert
            result.StatusCode.Should()
                .Be((int)System.Net.HttpStatusCode.OK);
        }


        [Fact]
        [Trait("POST - Operation", "Create")]
        public async Task Create_User_Fail()
        {

            string email = UserWebApiTest.Email;
            string firstName = FirstName;
            string lastName = FirstName;
            string password = Password;

            CreateUserViewModel user = new CreateUserViewModel
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Password = password
            };

            GetUserViewModel nonRegisteredUser = new GetUserViewModel
            {
                Success = false,
                Errors = new string[] { "Error" },
                Token = "token",
            };


            _userServiceMock.Setup(x => x.CreateAsync(It.IsAny<CreateUserViewModel>()))
                .Returns(Task.FromResult(nonRegisteredUser));

            //Act
            UserController userController = new UserController(_userServiceMock.Object, _loggerMock.Object);
            BadRequestObjectResult result = await userController.CreateUserAsync(user) as BadRequestObjectResult;


            //Assert
            result.StatusCode.Should()
                .Be((int)System.Net.HttpStatusCode.BadRequest);
        }
    
    
        [Fact]
        [Trait("POST - Operation", "Authenticate")]
        public async Task Authenticate_User_Success()
        {
            AuthenticateUserViewModel user = new AuthenticateUserViewModel
            {
                Email = UserWebApiTest.Email,
                Password = UserWebApiTest.Password
            };

            GetUserViewModel registeredUser = new GetUserViewModel
            {
                Email = UserWebApiTest.Email,
                FirstName = UserWebApiTest.FirstName,
                LastName = UserWebApiTest.LastName,
                Success = true,
                Id = 1,
                Token = "token",
                Errors = null
            };


            _userServiceMock.Setup(x => x.AuthenticateAsync(It.IsAny<AuthenticateUserViewModel>()))
                .Returns(Task.FromResult(registeredUser));

            //Act
            UserController userController = new UserController(_userServiceMock.Object, _loggerMock.Object);
            OkObjectResult result = await userController.AuthenticateUserAsync(user) as OkObjectResult;


            //Assert
            result.StatusCode.Should()
                .Be((int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        [Trait("POST - Operation", "Authenticate")]
        public async Task Authenticate_User_Fail()
        {
            AuthenticateUserViewModel user = new AuthenticateUserViewModel
            {
                Email = UserWebApiTest.Email,
                Password = UserWebApiTest.Password
            };

            GetUserViewModel registeredUser = new GetUserViewModel
            {
                Success = false,
                Errors = new string [] { "Error" }
            };


            _userServiceMock.Setup(x => x.AuthenticateAsync(It.IsAny<AuthenticateUserViewModel>()))
                .Returns(Task.FromResult(registeredUser));

            //Act
            UserController userController = new UserController(_userServiceMock.Object, _loggerMock.Object);
            BadRequestObjectResult result = await userController.AuthenticateUserAsync(user) as BadRequestObjectResult;


            //Assert
            result.StatusCode.Should()
                .Be((int)System.Net.HttpStatusCode.BadRequest);
        }
    }
}
