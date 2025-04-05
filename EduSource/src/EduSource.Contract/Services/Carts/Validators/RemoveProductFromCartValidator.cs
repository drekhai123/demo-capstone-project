using FluentValidation;

namespace EduSource.Contract.Services.Carts.Validators;

public class RemoveProductFromCartValidator : AbstractValidator<Command.RemoveProductFromCart>
{
    public RemoveProductFromCartValidator()
    {
        RuleFor(x => x.ProductId).NotNull().NotEmpty();
        RuleFor(x => x.AccountId).NotNull().NotEmpty();
    }
}
