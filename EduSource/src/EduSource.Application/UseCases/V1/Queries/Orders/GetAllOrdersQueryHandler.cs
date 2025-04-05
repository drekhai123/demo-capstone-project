using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Orders;
using EduSource.Domain.Abstraction.Dappers;
using EduSource.Domain.Entities;
using static EduSource.Contract.DTOs.OrderDTOs.OrderResponseDTO;
using static EduSource.Contract.Services.Orders.Response;
namespace EduSource.Application.UseCases.V1.Queries.Orders;

public sealed class GetAllOrdersQueryHandler : IQueryHandler<Query.GetAllOrdersQuery, Success<PagedResult<OrderResponse>>>
{
    private readonly IDPUnitOfWork _dpUnitOfWork;
    public GetAllOrdersQueryHandler(IDPUnitOfWork dpUnitOfWork)
    {
        _dpUnitOfWork = dpUnitOfWork;
    }

    public async Task<Result<Success<PagedResult<OrderResponse>>>> Handle(Query.GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var listOrders = await _dpUnitOfWork.OrderRepositories.GetAllOrdersByAdminAsync(request.PageIndex, request.PageSize, request.FilterParams, request.SelectedColumns);
        var listOrdersMapped = new List<OrderResponse>();
        var listOrderDetailsMapped = new List<List<OrderDetailsResponseDTO>>();
        listOrders.Items.ForEach(order =>
        {
            var listTemp = new List<OrderDetailsResponseDTO>();
            listOrderDetailsMapped.Add(listTemp);
            order.OrderDetails.ToList().ForEach(detail =>
            {
                var name = detail.Product != null ? detail.Product.Name : detail.ProductRequest.Name;
                var price = detail.Product != null ? detail.Product.Price : detail.ProductRequest.Price;

                listTemp.Add(new OrderDetailsResponseDTO(detail.Id, name, price, detail.Quantity));
            });
        });
        for (int i = 0; i < listOrders.Items.Count; i++)
        {
            listOrdersMapped.Add(new OrderResponse(listOrders.Items[i].Id, listOrders.Items[i].TotalPrice, listOrders.Items[i].TotalPrice, listOrders.Items[i].CreatedDate.Value, listOrders.Items[i].Description, listOrders.Items[i].OrderCode,
                new AccountResponseDTO(listOrders.Items[i].Account.Id, listOrders.Items[i].Account.FirstName, listOrders.Items[i].Account.LastName, listOrders.Items[i].Account.Email, listOrders.Items[i].Account.CropAvatarUrl, listOrders.Items[i].Account.GenderType),
                listOrderDetailsMapped[i]));
        }
        //listOrders.Items.ForEach(order =>
        //{
        //    listOrdersMapped.Add(new OrderResponse(order.Id, order.TotalPrice, order.TotalPrice, order.CreatedDate.Value, order.Description, order.OrderCode,
        //        new AccountResponseDTO(order.Account.Id, order.Account.FirstName, order.Account.LastName, order.Account.Email, order.Account.CropAvatarUrl, order.Account.GenderType),
        //        listOrderDetailsMapped[0]));
        //});
        //Mapping Category to CategoryResponse
        var result = new PagedResult<OrderResponse>(listOrdersMapped, listOrders.PageIndex, listOrders.PageSize, listOrders.TotalCount, listOrders.TotalPages);
        //Check if ListCategory is empty
        if (listOrders.Items.Count == 0)
        {
            return Result.Success(new Success<PagedResult<OrderResponse>>(MessagesList.OrdersNotFoundException.GetMessage().Code, MessagesList.OrdersNotFoundException.GetMessage().Message, result));
        }
        //Return result
        return Result.Success(new Success<PagedResult<OrderResponse>>(MessagesList.OrderGetAllOrdersSuccess.GetMessage().Code, MessagesList.OrderGetAllOrdersSuccess.GetMessage().Message, result));
    }
}
