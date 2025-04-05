using FluentValidation;

namespace EduSource.Contract.Services.Carts.Validators;

public class AddProductToCartValidator : AbstractValidator<Command.AddProductToCartCommand>
{
    public AddProductToCartValidator()
    {
        RuleFor(x => x.ProductId).NotNull().NotEmpty();
        RuleFor(x => x.AccountId).NotNull().NotEmpty();
    }
}
