using AutoMapper;
using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Products;
using EduSource.Domain.Abstraction.Dappers;
using System.Collections;
using static EduSource.Contract.Services.Books.Response;
using static EduSource.Contract.Services.Products.Response;

namespace EduSource.Application.UseCases.V1.Queries.Products;

public sealed class GetAllProductsPurchasedQueryHandler : IQueryHandler<Query.GetAllProductsPurchasedQuery, Success<PagedResult<ProductResponse>>>
{
    private readonly IDPUnitOfWork _dpUnitOfWork;
    public GetAllProductsPurchasedQueryHandler(IDPUnitOfWork dpUnitOfWork)
    {
        _dpUnitOfWork = dpUnitOfWork;
    }

    public async Task<Result<Success<PagedResult<ProductResponse>>>> Handle(Query.GetAllProductsPurchasedQuery request, CancellationToken cancellationToken)
    {
        var listProducts = await _dpUnitOfWork.ProductRepositories.GetProductsPurchasedAsync(request.PageIndex, request.PageSize, request.FilterParams, request.SelectedColumns);
        var listProductsMapped = new List<ProductResponse>();
        listProducts.Items.ForEach(product =>
        {
            listProductsMapped.Add(new ProductResponse(product.Id, product.Name, product.Price, product.Category, product.Unit, product.Description, product.ContentType, product.UploadType, product.TotalPage, product.Size, product.ImageUrl, product.FileUrl, product.Rating, product.IsPublic, product.IsApproved, null, null, true));
        });
        //Mapping Category to CategoryResponse
        var result = new PagedResult<ProductResponse>(listProductsMapped, listProducts.PageIndex, listProducts.PageSize, listProducts.TotalCount, listProducts.TotalPages);
        //Check if ListCategory is empty
        if (listProducts.Items.Count == 0)
        {
            return Result.Success(new Success<PagedResult<ProductResponse>>(MessagesList.ProductsNotFoundException.GetMessage().Code, MessagesList.ProductsNotFoundException.GetMessage().Message, result));
        }
        //Return result
        return Result.Success(new Success<PagedResult<ProductResponse>>(MessagesList.ProductGetAllProductsPurchasedSuccess.GetMessage().Code, MessagesList.ProductGetAllProductsPurchasedSuccess.GetMessage().Message, result));
    }
}
