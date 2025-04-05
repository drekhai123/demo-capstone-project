using AutoMapper;
using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Carts;
using EduSource.Domain.Abstraction.Dappers;
using System.Collections;
using static EduSource.Contract.Services.Books.Response;
using static EduSource.Contract.Services.Products.Response;

namespace EduSource.Application.UseCases.V1.Queries.Carts;

public sealed class GetAllProductFromCartQueryHandler : IQueryHandler<Query.GetAllProductsFromCartQuery, Success<PagedResult<ProductResponse>>>
{
    private readonly IDPUnitOfWork _dpUnitOfWork;
    public GetAllProductFromCartQueryHandler(IDPUnitOfWork dpUnitOfWork)
    {
        _dpUnitOfWork = dpUnitOfWork;
    }

    public async Task<Result<Success<PagedResult<ProductResponse>>>> Handle(Query.GetAllProductsFromCartQuery request, CancellationToken cancellationToken)
    {
        var listProductsInCart = await _dpUnitOfWork.ProductRepositories.GetProductsInCartAsync(request.PageIndex, request.PageSize, request.FilterParams, request.SelectedColumns);
        var listProductsInCartMapped = new List<ProductResponse>();
        listProductsInCart.Items.ForEach(product =>
        {
            listProductsInCartMapped.Add(new ProductResponse(product.Id, product.Name, product.Price, product.Category, product.Unit, product.Description, product.ContentType, product.UploadType, product.TotalPage, product.Size, product.ImageUrl, product.FileDemoUrl, product.Rating, product.IsPublic, product.IsApproved, null, null, false));
        });
        //Mapping Category to CategoryResponse
        var result = new PagedResult<ProductResponse>(listProductsInCartMapped, listProductsInCart.PageIndex, listProductsInCart.PageSize, listProductsInCart.TotalCount, listProductsInCart.TotalPages);
        //Check if ListCategory is empty
        if (listProductsInCart.Items.Count == 0)
        {
            return Result.Success(new Success<PagedResult<ProductResponse>>(MessagesList.CartProductsNotFoundInCartException.GetMessage().Code, MessagesList.CartProductsNotFoundInCartException.GetMessage().Message, result));
        }
        //Return result
        return Result.Success(new Success<PagedResult<ProductResponse>>(MessagesList.CartGetAllProductsFromCartSuccess.GetMessage().Code, MessagesList.CartGetAllProductsFromCartSuccess.GetMessage().Message, result));
    }
}
