using EduSource.Contract.Enumarations.Product;

namespace EduSource.Contract.Services.Products;

public static class Filter
{
    public record ProductFilter(string? Name, double? Price, CategoryType? Category, string? Description, ContentType? ContentType, int? Unit, UploadType? UploadType, int? TotalPage, double? Size, double? Rating, bool? IsPublic, bool? IsApproved, Guid? StaffId, Guid? BookId, Guid? UserId);
}