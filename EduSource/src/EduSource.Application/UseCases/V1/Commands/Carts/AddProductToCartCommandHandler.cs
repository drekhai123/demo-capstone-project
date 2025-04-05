using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Carts;
using EduSource.Domain.Abstraction.EntitiyFramework;
using EduSource.Domain.Entities;
using EduSource.Domain.Exceptions;

namespace EduSource.Application.UseCases.V1.Commands.Carts;

public sealed class AddProductToCartCommandHandler : ICommandHandler<Command.AddProductToCartCommand>
{
    private readonly IEFUnitOfWork _efUnitOfWork;

    public AddProductToCartCommandHandler(IEFUnitOfWork efUnitOfWork)
    {
        _efUnitOfWork = efUnitOfWork;
    }

    public async Task<Result> Handle(Command.AddProductToCartCommand request, CancellationToken cancellationToken)
    {
        // Find Product
        var productFound = await _efUnitOfWork.ProductRepository.FindByIdAsync(request.ProductId);
        if(productFound == null)
        {
            throw new ProductException.ProductNotFoundException();
        }
        // Find Account
        var accountFound = await _efUnitOfWork.AccountRepository.FindByIdAsync(request.AccountId);
        if(accountFound == null)
        {
            throw new AccountException.AccountNotFoundException();
        }
        // Check if User has already added product to cart
        var productInCart = await _efUnitOfWork.CartRepository.FindSingleAsync(x => x.ProductId == request.ProductId && x.AccountId == request.AccountId);
        if(productInCart != null)
        {
            throw new CartException.ProductHasAlreadyAddedToCartException();
        }

        // Add Product to cart
        var productToCart = Cart.AddProductToCart(request.AccountId, request.ProductId);
        _efUnitOfWork.CartRepository.Add(productToCart);
        await _efUnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new Success(MessagesList.CartAddedProductToCartSuccess.GetMessage().Code, MessagesList.CartAddedProductToCartSuccess.GetMessage().Message));
    }
}
