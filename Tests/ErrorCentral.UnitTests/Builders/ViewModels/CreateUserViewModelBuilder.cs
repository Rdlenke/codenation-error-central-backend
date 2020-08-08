using ErrorCentral.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.UnitTests.Builders.ViewModels
{
    public class CreateUserViewModelBuilder : IBuilder<CreateUserViewModel>
    {
        private CreateUserViewModel _createUserViewModel;

        public string Email => "user@user.com";
        public string Password => "MSIKm342Oas!";
        public string FirstName => "User";
        public string LastName => "User";

        public CreateUserViewModelBuilder()
        {
            _createUserViewModel = WithDefaultValues();
        }

        public CreateUserViewModel Build()
        {
            return _createUserViewModel;
        }

        private CreateUserViewModel WithDefaultValues()
        {
            return new CreateUserViewModel()
            {
                Email = Email,
                FirstName = FirstName,
                Password = Password,
                LastName = LastName
            };
        }
    }
}
