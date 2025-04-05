using FluentValidation;

namespace EduSource.Contract.Services.Orders.Validators;

public class GetDashboardValidator : AbstractValidator<Query.GetDashboardQuery>
{
    public GetDashboardValidator()
    {
        RuleFor(x => x.year)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.month)
            .NotNull()
            .When(x => x.week.HasValue)
            .WithMessage("Month must have a value if Week is specified.");
    }

}
