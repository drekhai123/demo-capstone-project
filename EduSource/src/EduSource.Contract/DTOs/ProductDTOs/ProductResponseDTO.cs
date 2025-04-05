using EduSource.Contract.Enumarations.Book;

namespace EduSource.Contract.DTOs.ProductDTOs;

public static class ProductResponseDTO
{
    public sealed class BookResponse
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int GradeLevel { get; set; }
        public CategoryType Category { get; set; }
    }
}
