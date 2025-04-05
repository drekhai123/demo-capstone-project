using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Products;
using EduSource.Domain.Abstraction.Dappers;
using EduSource.Domain.Exceptions;
using static EduSource.Contract.DTOs.ProductDTOs.ProductResponseDTO;
using static EduSource.Contract.Services.Products.Response;

namespace EduSource.Application.UseCases.V1.Queries.Products;

public sealed class GetProductByIdByUserQueryHandler : IQueryHandler<Query.GetProductByIdByUserQuery, Success<ProductResponse>>
{
    private readonly IDPUnitOfWork _dpUnitOfWork;

    public GetProductByIdByUserQueryHandler(IDPUnitOfWork dpUnitOfWork)
    {
        _dpUnitOfWork = dpUnitOfWork;
    }

    public async Task<Result<Success<ProductResponse>>> Handle(Query.GetProductByIdByUserQuery request, CancellationToken cancellationToken)
    {
        //Find Product By Id
        var productFound = await _dpUnitOfWork.ProductRepositories.GetDetailsAsync(request.Id);
        if (productFound == null)
        {
            throw new ProductException.ProductNotFoundException();
        }
        //Check if Product has been purchased by User or not
        var isProductPurchased = await _dpUnitOfWork.ProductRepositories.IsProductPurchasedByUserAsync(request.Id, request.AccountId);
        var numberOfImagesOfProduct = productFound.ImageOfProducts.Count;
        var listImageOfProducts = isProductPurchased ? productFound.ImageOfProducts.ToList().Select(i => i.ImageUrl).ToList() 
            : numberOfImagesOfProduct > 3 ? productFound.ImageOfProducts.ToList().Select(x => x.ImageUrl).ToList().Take(3).ToList() 
                : productFound.ImageOfProducts.ToList().Select(x => x.ImageUrl).ToList().Take(numberOfImagesOfProduct - 1).ToList();
        var fileUrl = isProductPurchased ? productFound.FileUrl : productFound.FileDemoUrl;
        var result = new ProductResponse(productFound.Id, productFound.Name, productFound.Price, productFound.Category, productFound.Unit, productFound.Description, productFound.ContentType, productFound.UploadType, productFound.TotalPage, productFound.Size, productFound.ImageUrl, fileUrl, productFound.Rating, productFound.IsPublic, productFound.IsApproved, listImageOfProducts, new BookResponse()
        {
            Name = productFound.Book.Name,
            Category = productFound.Book.Category,
            ImageUrl = productFound.Book.ImageUrl,
            GradeLevel = productFound.Book.GradeLevel,
        }, isProductPurchased);
        return Result.Success(new Success<ProductResponse>(MessagesList.ProductGetDetailsProductByUserSuccess.GetMessage().Code, MessagesList.ProductGetDetailsProductByUserSuccess.GetMessage().Message, result));
    }
}
