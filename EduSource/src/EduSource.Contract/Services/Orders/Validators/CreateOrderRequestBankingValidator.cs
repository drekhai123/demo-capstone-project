using FluentValidation;
using EduSource.Contract.Services.Orders;

namespace EduSource.Contract.Services.Orders.Validators;

internal class CreateOrderRequestBankingValidator : AbstractValidator<Command.CreateOrderRequestBankingCommand>
{
    public CreateOrderRequestBankingValidator()
    {
        RuleFor(x => x.AccountId).NotNull().NotEmpty();
        RuleFor(x => x.ProductRequestId).NotNull().NotEmpty();
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.Price).NotNull().NotEmpty(); 
    }
}
