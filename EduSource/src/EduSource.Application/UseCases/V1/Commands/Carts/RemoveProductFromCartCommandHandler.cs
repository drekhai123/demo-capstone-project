using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Carts;
using EduSource.Domain.Abstraction.EntitiyFramework;
using EduSource.Domain.Entities;
using EduSource.Domain.Exceptions;

namespace EduSource.Application.UseCases.V1.Commands.Carts;

public sealed class RemoveProductFromCartCommandHandler : ICommandHandler<Command.RemoveProductFromCart>
{
    private readonly IEFUnitOfWork _efUnitOfWork;

    public RemoveProductFromCartCommandHandler(IEFUnitOfWork efUnitOfWork)
    {
        _efUnitOfWork = efUnitOfWork;
    }

    public async Task<Result> Handle(Command.RemoveProductFromCart request, CancellationToken cancellationToken)
    {
        // Find Product
        var productFound = await _efUnitOfWork.ProductRepository.FindByIdAsync(request.ProductId);
        if (productFound == null)
        {
            throw new ProductException.ProductNotFoundException();
        }
        // Find Account
        var accountFound = await _efUnitOfWork.AccountRepository.FindByIdAsync(request.AccountId);
        if (accountFound == null)
        {
            throw new AccountException.AccountNotFoundException();
        }
        // Check if Product has existed in User's Cart
        var productInCart = await _efUnitOfWork.CartRepository.FindSingleAsync(x => x.ProductId == request.ProductId && x.AccountId == request.AccountId);
        if (productInCart == null)
        {
            throw new CartException.ProductNotFoundInCartException();
        }

        // Remove Product to cart
        _efUnitOfWork.CartRepository.Remove(productInCart);
        await _efUnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new Success(MessagesList.CartRemovedProductFromCartSuccess.GetMessage().Code, MessagesList.CartRemovedProductFromCartSuccess.GetMessage().Message));
    }
}
