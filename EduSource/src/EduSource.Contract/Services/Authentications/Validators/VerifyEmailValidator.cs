using FluentValidation;

namespace EduSource.Contract.Services.Authentications.Validators;
public class VerifyEmailValidator : AbstractValidator<Command.VerifyEmailCommand>
{
    public VerifyEmailValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
