using FluentValidation;
using EduSource.Contract.Services.Orders;

namespace EduSource.Contract.Services.Orders.Validators;

internal class CreateOrderBankingValidator : AbstractValidator<Command.CreateOrderBankingCommand>
{
    public CreateOrderBankingValidator()
    {
        RuleFor(x => x.AccountId).NotNull().NotEmpty();
        RuleFor(x => x.ProductIds)
            .NotNull().WithMessage("Ids list must not be null.")
            .NotEmpty().WithMessage("Cannot checkout with an empty list of products!")
            .Must((order, productIds) => order.IsFromCart || productIds.Count == 1)
            .WithMessage("Only checkout 1 Product if checking out directly!");

        RuleFor(x => x.IsFromCart).NotNull();
    }
}
