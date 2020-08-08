﻿using ErrorCentral.Application.ViewModels.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.Application.ViewModels.Validators
{
    public class CreateUserViewModelValidation : AbstractValidator<CreateUserViewModel>
    {
        public CreateUserViewModelValidation()
        {
            RuleFor(x => x.Email).EmailValidator();
            RuleFor(x => x.Password).PasswordValidator();

            RuleFor(x => x.FirstName).FirstNameValidator();
            RuleFor(x => x.LastName).LastNameValidator();
        }
    }
}
