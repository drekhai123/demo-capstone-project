using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.DTOs.OrderDTOs;
using EduSource.Contract.DTOs.PaymentDTOs;
using EduSource.Contract.Services.Orders;
using EduSource.Contract.Settings;
using EduSource.Domain.Abstraction.Dappers;
using EduSource.Domain.Abstraction.EntitiyFramework;
using EduSource.Domain.Entities;
using EduSource.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EduSource.Application.UseCases.V1.Commands.Orders;

public sealed class OrderRequestSuccessCommandHandler : ICommandHandler<Command.OrderRequestSuccessCommand, Success<Response.OrderRequestSuccess>>
{
    private readonly IEFUnitOfWork _efUnitOfWork;
    private readonly IDPUnitOfWork _dpUnitOfWork;
    private readonly IPublisher _publisher;
    private readonly IResponseCacheService _responseCacheService;
    private readonly ClientSetting _clientSetting;

    public OrderRequestSuccessCommandHandler(IEFUnitOfWork efUnitOfWork, IDPUnitOfWork dPUnitOfWork, IPublisher publisher, IResponseCacheService responseCacheService, IOptions<ClientSetting> clientConfiguration)
    {
        _efUnitOfWork = efUnitOfWork;
        _dpUnitOfWork = dPUnitOfWork;
        _publisher = publisher;
        _responseCacheService = responseCacheService;
        _clientSetting = clientConfiguration.Value;
    }
    public async Task<Result<Success<Response.OrderRequestSuccess>>> Handle(Command.OrderRequestSuccessCommand request, CancellationToken cancellationToken)
    {
        // Get infomation saved in memory
        var orderMemory = await _responseCacheService.GetCacheResponseAsync($"order_{request.OrderId}");
        // Conver JSON to object
        var orderObject = JsonConvert.DeserializeObject<ResultCacheDTO.ProductRequestPaymentCacheDTO>(orderMemory);

        // Find User
        var accountFound = await _efUnitOfWork.AccountRepository.FindByIdAsync(orderObject.AccountId) ?? throw new AccountException.AccountNotFoundException();
        // Calculate the sum of order
        var sumOfOrder = orderObject.Price;
        // Create ProductRequest
        var productRequestCreated = ProductRequest.CreateProductRequest(orderObject.ProductRequestId, orderObject.Name, orderObject.Price, orderObject.AccountId);
        _efUnitOfWork.ProductRequestRepository.Add(productRequestCreated);
        // Create Order
        var orderId = Guid.NewGuid();
        var orderCreated = Order.CreateOrder(orderId, sumOfOrder, orderObject.OrderCode, orderObject.Description, orderObject.AccountId);
        _efUnitOfWork.OrderRepository.Add(orderCreated);
        // Create OrderDetails for ProductRequest of order
        var orderDetail = OrderDetails.CreateOrderDetailsWithProductRequest(1, orderCreated.Id, orderObject.ProductRequestId);
        _efUnitOfWork.OrderDetailsRepository.Add(orderDetail);
        
        await _efUnitOfWork.SaveChangesAsync(cancellationToken);
        // Delete cache order
        await _responseCacheService.DeleteCacheResponseAsync($"order_{request.OrderId}");
        var result = new Response.OrderRequestSuccess($"{_clientSetting.Url}{_clientSetting.OrderRequest}/{orderObject.ProductRequestId}?is_success=1");
        return Result.Success(new Success<Response.OrderRequestSuccess>("", "", result));
    }
}
