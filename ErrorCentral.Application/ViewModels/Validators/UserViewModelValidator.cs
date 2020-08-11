using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ErrorCentral.Application.ViewModels.Validators
{
    public static class UserViewModelValidator
    {
        public static IRuleBuilderOptions<T, string> EmailValidator<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.EmailAddress().WithMessage("Should be an email address!");
        }

        public static IRuleBuilderOptions<T, string> PasswordValidator<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$")
                .WithMessage("Password length must have more than 8 characters, with at least one digit, one uppercase character, one lowercase character "
                             + "and one special character");
        }

        public static IRuleBuilderOptions<T, string> FirstNameValidator<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Matches(@"^\S*$").WithMessage("First Name shouldn't contain whitespaces");
        }
        public static IRuleBuilderOptions<T, string> LastNameValidator<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Matches(@"^\S*$").WithMessage("Last Name shouldn't contain whitespaces");
        }
    }
}
