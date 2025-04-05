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

public sealed class OrderSuccessCommandHandler : ICommandHandler<Command.OrderSuccessCommand, Success<Response.OrderSuccess>>
{
    private readonly IEFUnitOfWork _efUnitOfWork;
    private readonly IDPUnitOfWork _dpUnitOfWork;
    private readonly IPublisher _publisher;
    private readonly IResponseCacheService _responseCacheService;
    private readonly ClientSetting _clientSetting;

    public OrderSuccessCommandHandler(IEFUnitOfWork efUnitOfWork, IDPUnitOfWork dPUnitOfWork, IPublisher publisher, IResponseCacheService responseCacheService, IOptions<ClientSetting> clientConfiguration)
    {
        _efUnitOfWork = efUnitOfWork;
        _dpUnitOfWork = dPUnitOfWork;
        _publisher = publisher;
        _responseCacheService = responseCacheService;
        _clientSetting = clientConfiguration.Value;
    }
    public async Task<Result<Success<Response.OrderSuccess>>> Handle(Command.OrderSuccessCommand request, CancellationToken cancellationToken)
    {
        // Get infomation saved in memory
        var orderMemory = await _responseCacheService.GetCacheResponseAsync($"order_{request.OrderId}");
        // Conver JSON to object
        var orderObject = JsonConvert.DeserializeObject<ResultCacheDTO.ProductPaymentCacheDTO>(orderMemory);


        // Find User
        var accountFound = await _efUnitOfWork.AccountRepository.FindByIdAsync(orderObject.AccountId) ?? throw new AccountException.AccountNotFoundException();
        if (!orderObject.IsFromCart)
        {
            var productCheckout = await _efUnitOfWork.ProductRepository.FindByIdAsync(orderObject.ProductIds[0]) ?? throw new ProductException.ProductNotFoundException();
            // Calculate the sum of order
            var sumOfOrder = productCheckout.Price;
            // Create Order
            var orderId = Guid.NewGuid();
            var orderCreated = Order.CreateOrder(orderId, sumOfOrder, orderObject.OrderCode, orderObject.Description, orderObject.AccountId);
            _efUnitOfWork.OrderRepository.Add(orderCreated);
            // Create OrderDetails for Product of order
            var orderDetail = OrderDetails.CreateOrderDetailsWithProduct(1, orderCreated.Id, productCheckout.Id);                  
            _efUnitOfWork.OrderDetailsRepository.Add(orderDetail);
            // Check Product in Cart or not, if in Cart then remove
            var productInCart = await _efUnitOfWork.CartRepository.FindSingleAsync(x => x.ProductId == productCheckout.Id && x.AccountId == orderObject.AccountId);
            if (productInCart != null) _efUnitOfWork.CartRepository.Remove(productInCart);
            await _efUnitOfWork.SaveChangesAsync(cancellationToken);
            // Delete cache order
            await _responseCacheService.DeleteCacheResponseAsync($"order_{request.OrderId}");
            // Create List InvoiceItems for email
            var invoiceItems = new List<EmailOrderDTO>()
            {
                new EmailOrderDTO()
                {
                    Name = productCheckout.Name,
                    Price = productCheckout.Price,
                    Quantity = 1,
                    Total = productCheckout.Price,
                }
            };
            // Send success order email and invoice for User
            await Task.WhenAll(
               _publisher.Publish(new DomainEvent.NotiUserOrderSuccess(orderCreated.Id, accountFound.Email, orderObject.OrderCode.ToString(), DateTime.UtcNow.ToString(), invoiceItems, sumOfOrder), cancellationToken)
            );
            var result = new Response.OrderSuccess($"{_clientSetting.Url}{_clientSetting.OrderSuccess}");
            return Result.Success(new Success<Response.OrderSuccess>("", "", result));
        }
        else
        {
            var productsCheckout = await _dpUnitOfWork.ProductRepositories.GetProductsInCartByListIdsAsync(orderObject.AccountId, orderObject.ProductIds);
            //Check if productCheckout match product in cart
            orderObject.ProductIds.ForEach(id =>
            {
                if (!productsCheckout.Any(x => x.Id == id))
                {
                    throw new CartException.ProductNotInCartException();
                }
            });
            // Calculate the sum of order
            var sumOfOrder = productsCheckout.Sum(p => p.Price);
            // Create Order
            var orderId = Guid.NewGuid();
            var orderCreated = Order.CreateOrder(orderId, sumOfOrder, orderObject.OrderCode, orderObject.Description, orderObject.AccountId);
            _efUnitOfWork.OrderRepository.Add(orderCreated);
            // Create OrderDetails for Product of order
            var listOrderDetails = new List<OrderDetails>();
            productsCheckout.ToList().ForEach(p =>
            {
                listOrderDetails.Add(OrderDetails.CreateOrderDetailsWithProduct(1, orderId, p.Id));
            });
            _efUnitOfWork.OrderDetailsRepository.AddRange(listOrderDetails);
            // Delete Products in Cart
            var listProductIds = productsCheckout.Select(x => x.Id).ToList();
            var cartItems = await _efUnitOfWork.CartRepository.FindAllAsync(
                x => x.AccountId == orderObject.AccountId && x.ProductId.HasValue && listProductIds.Contains(x.ProductId.Value)
            );
            _efUnitOfWork.CartRepository.RemoveMultiple(cartItems.ToList());
            await _efUnitOfWork.SaveChangesAsync(cancellationToken);
            // Delete cache order
            await _responseCacheService.DeleteCacheResponseAsync($"order_{request.OrderId}");
            // Create List InvoiceItems for email
            var invoiceItems = new List<EmailOrderDTO>();
            productsCheckout.ToList().ForEach(p =>
            {
                invoiceItems.Add(new EmailOrderDTO()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = 1,
                    Total = p.Price
                });
            });
            // Send success order email and invoice for User
            await Task.WhenAll(
               _publisher.Publish(new DomainEvent.NotiUserOrderSuccess(orderCreated.Id, accountFound.Email, orderObject.OrderCode.ToString(), DateTime.UtcNow.ToString(), invoiceItems, sumOfOrder), cancellationToken)
            );
            var result = new Response.OrderSuccess($"{_clientSetting.Url}{_clientSetting.OrderSuccess}");
            return Result.Success(new Success<Response.OrderSuccess>("", "", result));
        }       
    }
}
