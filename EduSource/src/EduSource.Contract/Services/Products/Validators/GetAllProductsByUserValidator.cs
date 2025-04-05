using FluentValidation;

namespace EduSource.Contract.Services.Products.Validators;

public class GetAllProductsByUserValidator : AbstractValidator<Query.GetAllProductsByUserQuery>
{
    public GetAllProductsByUserValidator()
    {
        RuleFor(query => query.FilterParams)
            .Must(param => param.ContentType == null || (param.ContentType != Enumarations.Product.ContentType.Review || !param.Unit.HasValue))
            .WithMessage("If ContentType is Review, Unit must not be provided.");
    }

}
