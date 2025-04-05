using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Enumarations.Product;
using Microsoft.AspNetCore.Http;

namespace EduSource.Contract.Services.Products;
public static class Command
{
    public record CreateProductCommand(string Name, int Price, CategoryType Category, string Description, ContentType ContentType, int? Unit, UploadType UploadType, int TotalPage, double Size, IFormFile MainImage, IFormFile File, IFormFile FileDemo, List<IFormFile> OtherImages, Guid BookId, Guid AccountId) : ICommand;
}
