using EduSource.Contract.Enumarations.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSource.Contract.DTOs.ProductDTOs
{
    public static class ProductRequestDTO
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
            public bool? IsPublic { get; set; }
            public bool? IsApproved { get; set; }
            public Guid? BookId { get; set; }
        }

        public sealed class GetAllProductPurchasedRequestDTO
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
            public bool? IsPublic { get; set; }
            public bool? IsApproved { get; set; }
            public Guid? BookId { get; set; }
        }

        public sealed class CreateProductRequestDTO
        {
            public string Name { get; set; }
            public int Price { get; set; }
            public CategoryType Category { get; set; }
            public string Description { get; set; }
            public ContentType ContentType { get; set; }
            public int Unit { get; set; }
            public UploadType UploadType { get; set; }
            public int TotalPage { get; set; }
            public double Size { get; set; }
            public IFormFile MainImage { get; set; }
            public IFormFile File { get; set; }
            public IFormFile FileDemo { get; set; }
            public List<IFormFile> OtherImages { get; set; }
            public Guid BookId { get; set; }
        }
    }
}
