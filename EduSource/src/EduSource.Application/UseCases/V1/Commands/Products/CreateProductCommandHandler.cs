using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Products;
using EduSource.Domain.Abstraction.EntitiyFramework;
using EduSource.Domain.Entities;
using EduSource.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System.IO;
namespace EduSource.Application.UseCases.V1.Commands.Products;

public sealed class CreateProductCommandHandler : ICommandHandler<Command.CreateProductCommand>
{
    private readonly IEFUnitOfWork _efUnitOfWork;
    private readonly IMediaService _mediaService;

    public CreateProductCommandHandler(IEFUnitOfWork efUnitOfWork, IMediaService mediaService)
    {
        _efUnitOfWork = efUnitOfWork;
        _mediaService = mediaService;
    }

    public async Task<Result> Handle(Command.CreateProductCommand request, CancellationToken cancellationToken)
    {
        //Find Account of Staff
        var accountStaffFound = await _efUnitOfWork.AccountRepository.FindByIdAsync(request.AccountId);
        if (accountStaffFound == null)
        {
            throw new AccountException.AccountNotFoundException();
        }
        //Find Book
        var bookFound = await _efUnitOfWork.BookRepository.FindByIdAsync(request.BookId);
        if (bookFound == null)
        {
            throw new BookException.BookNotFoundException();
        }
        //Upload Main Image and get Url
        var uploadMainImage = await _mediaService.UploadImageAsync($"main_image_{request.Name}" ,request.MainImage);
        //Upload File and get Url
        var uploadFile = await _mediaService.UploadImageAsync(request.File.FileName, request.File);
        //Upload Demo File and get Url
        var uploadFileDemo = await _mediaService.UploadImageAsync(request.FileDemo.FileName, request.FileDemo);

        var productCreated = Product.CreateProduct(request.Name, request.Price, request.Category, request.Description, request.ContentType, request.Unit.Value, request.UploadType, request.TotalPage, request.Size, uploadMainImage.PublicImageId, uploadMainImage.ImageUrl, uploadFile.PublicImageId, uploadFile.ImageUrl, uploadFileDemo.PublicImageId, uploadFileDemo.ImageUrl, request.BookId, request.AccountId);
        _efUnitOfWork.ProductRepository.Add(productCreated);
        //Upload Other Image and get Url
        var uploadOtherImages = await _mediaService.UploadImagesAsync(request.OtherImages);
        _efUnitOfWork.ImageOfProductRepository.AddRange(uploadOtherImages.Select(x => ImageOfProduct.CreateImageOfProduct(x.PublicImageId, x.ImageUrl, productCreated.Id)).ToList());
        await _efUnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(new Success(MessagesList.ProductCreateProductSuccess.GetMessage().Code, MessagesList.ProductCreateProductSuccess.GetMessage().Message));
    }
}
