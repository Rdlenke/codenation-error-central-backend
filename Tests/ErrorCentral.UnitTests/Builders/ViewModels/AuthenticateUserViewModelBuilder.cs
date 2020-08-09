using ErrorCentral.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.UnitTests.Builders.ViewModels
{
    public class AuthenticateUserViewModelBuilder : IBuilder<AuthenticateUserViewModel>
    {
        private AuthenticateUserViewModel _authenticateUserViewModel;
        public string Email => "user@user.com";
        public string Password => "MSIKm342Oas!";


        public AuthenticateUserViewModelBuilder()
        {
            _authenticateUserViewModel = WithDefaultValues();
        }

        private AuthenticateUserViewModel WithDefaultValues()
        {
            return new AuthenticateUserViewModel()
            {
                Email = Email,
                Password = Password
            };
        }

        public AuthenticateUserViewModel Build()
        {
            return _authenticateUserViewModel;
        }
    }
}
