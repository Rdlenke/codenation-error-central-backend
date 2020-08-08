using ErrorCentral.Application.ViewModels.LogError;
using FluentValidation;

namespace ErrorCentral.Application.ViewModels.Validators
{
    public class CreateLogErrorViewModelValidator : AbstractValidator<CreateLogErrorViewModel>
    {
        public CreateLogErrorViewModelValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull().WithMessage("UserId cannot be null")
                .GreaterThan(0).WithMessage("UserId must be greater than 0");
            RuleFor(x => x.Title)
                .NotNull().WithMessage("Title cannot be null")
                .NotEmpty().WithMessage("Title cannot be empty")
                .MaximumLength(500).WithMessage("Title cannot be greater than 500");
            RuleFor(x => x.Source)
                .NotNull().WithMessage("Source cannot be null")
                .NotEmpty().WithMessage("Source cannot be empty");
            RuleFor(x => x.Level)
                .NotNull().WithMessage("Level cannot be null")
                .NotEmpty().WithMessage("Level cannot be empty")
                .IsInEnum().WithMessage("Level Informed value cannot be assigned");
            RuleFor(x => x.Environment)
                .NotNull().WithMessage("Environment cannot be null")
                .NotEmpty().WithMessage("Environment cannot be empty")
                .IsInEnum().WithMessage("Environment Informed value cannot be assigned");
        }
    }
}