using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSource.Contract.Services.Accounts.Validator;

public class UpdateEmailCommandValidator : AbstractValidator<Command.UpdateEmailCommand>
{
    public UpdateEmailCommandValidator() 
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
