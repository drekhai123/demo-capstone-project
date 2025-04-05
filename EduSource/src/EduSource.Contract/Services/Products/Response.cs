using EduSource.Contract.Enumarations.Product;
using static EduSource.Contract.DTOs.ProductDTOs.ProductResponseDTO;

namespace EduSource.Contract.Services.Products;

public static class Response
{
    public record ProductResponse(Guid Id, string Name, int Price, CategoryType Category, int? Unit, string? Description, ContentType ContentType, UploadType UploadType, int TotalPage, double Size, string ImageUrl, string FileUrl, double Rating, bool IsPublic, bool IsApproved, List<string>? listImages, BookResponse? book, bool isPurchased);
}
