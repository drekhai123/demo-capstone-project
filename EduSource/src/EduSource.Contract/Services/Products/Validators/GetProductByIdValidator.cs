using FluentValidation;

namespace EduSource.Contract.Services.Products.Validators;

public class GetProductByIdValidator : AbstractValidator<Query.GetProductByIdQuery>
{
    public GetProductByIdValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}
