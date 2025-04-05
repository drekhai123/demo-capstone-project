using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Enumarations.Product;
using Microsoft.AspNetCore.Http;

namespace EduSource.Contract.Services.Carts;
public static class Command
{
    public record AddProductToCartCommand(Guid ProductId, Guid AccountId) : ICommand;
    public record RemoveProductFromCart(Guid ProductId, Guid AccountId) : ICommand;

}
