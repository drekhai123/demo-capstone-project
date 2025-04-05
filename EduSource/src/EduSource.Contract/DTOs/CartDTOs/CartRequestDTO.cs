using EduSource.Contract.Enumarations.Product;

namespace EduSource.Contract.DTOs.CartDTOs;

public static class CartRequestDTO
{
    public sealed class GetAllProductRequestDTO
    {  
            public string? Name { get; set; }
            public double? Price { get; set; }
            public CategoryType? Category { get; set; }
            public string? Description { get; set; }
            public ContentType? ContentType { get; set; }
            public int? Unit { get; set; }
            public UploadType? UploadType { get; set; }
            public int? TotalPage { get; set; }
            public double? Size { get; set; }
            public double? Rating { get; set; }
            public Guid? BookId { get; set; }
    }
}
