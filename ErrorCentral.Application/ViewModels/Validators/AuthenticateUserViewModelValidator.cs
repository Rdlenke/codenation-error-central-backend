using ErrorCentral.Application.ViewModels.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ErrorCentral.Application.ViewModels.Validators
{
    public class AuthenticateUserViewModelValidator : AbstractValidator<AuthenticateUserViewModel>
    {
        public AuthenticateUserViewModelValidator()
        {
            RuleFor(x => x.Email).EmailValidator();
        }
    }
}
