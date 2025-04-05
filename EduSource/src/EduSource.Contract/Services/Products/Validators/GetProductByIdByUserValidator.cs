using FluentValidation;

namespace EduSource.Contract.Services.Products.Validators;

public class GetProductByIdByUserValidator : AbstractValidator<Query.GetProductByIdByUserQuery>
{
    public GetProductByIdByUserValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
        RuleFor(x => x.AccountId).NotNull().NotEmpty();
    }
}
