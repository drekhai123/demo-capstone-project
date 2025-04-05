using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.DTOs.OrderDTOs;

namespace EduSource.Contract.Services.Orders;
public static class Command
{
    public record CreateOrderBankingCommand(Guid AccountId, List<Guid> ProductIds, bool IsFromCart) : ICommand;
    public record OrderSuccessCommand(long OrderId) : ICommand<Success<Response.OrderSuccess>>;
    public record OrderFailCommand(long OrderId) : ICommand<Success<Response.OrderFail>>;

    public record CreateOrderRequestBankingCommand(Guid AccountId, int ProductRequestId, string Name, int Price) : ICommand;
    public record OrderRequestSuccessCommand(long OrderId) : ICommand<Success<Response.OrderRequestSuccess>>;
    public record OrderRequestFailCommand(long OrderId) : ICommand<Success<Response.OrderRequestFail>>;
}

