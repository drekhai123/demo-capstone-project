using EduSource.Contract.Enumarations.Product;
using EduSource.Domain.Abstraction.Entities;
using Microsoft.AspNetCore.Http;

namespace EduSource.Domain.Entities;

public class Product : DomainEntity<Guid>
{
    public Product()
    {

    }

    public string Name { get; private set; }
    public int Price { get; private set; }
    public CategoryType Category { get; private set; }
    public string Description { get; private set; }
    public ContentType ContentType { get; private set; }
    public int? Unit { get; private set; }
    public UploadType UploadType { get; private set; }
    public int TotalPage { get; private set; }
    public double Size { get; private set; }
    public string ImageId { get; private set; }
    public string ImageUrl { get; private set; }
    public string FileId { get; private set; }
    public string FileUrl { get; private set; }
    public string FileDemoId { get; private set; }
    public string FileDemoUrl { get; private set; }
    public double Rating { get; private set; }
    public bool IsPublic { get; private set; }
    public bool IsApproved { get; private set; }
    public Guid AccountId { get; private set; }
    public virtual Account Account { get; private set; }
    public Guid BookId { get; private set; }
    public virtual Book Book { get; private set; }
    public virtual ICollection<Cart> Carts { get; private set; }
    public virtual ICollection<Wishlist> Wishlists { get; private set; }
    public virtual ICollection<OrderDetails> OrderDetails { get; private set; }
    public virtual ICollection<ProductInCombo> ProductInCombos { get; private set; }
    public virtual ICollection<ImageOfProduct> ImageOfProducts { get; private set; }
    public virtual ICollection<Feedback> Feedbacks { get; private set; }

    public static Product CreateProductForSeedData(Guid id, string name, int price, CategoryType category, string description, ContentType contentType, int unit, UploadType uploadType, int totalPage, double size, string imageId, string imageUrl, string fileId, string fileUrl, string fileDemoId, string fileDemoUrl, Guid bookId, Guid accountId)
    {
        if (contentType == ContentType.Unit)
        {
            return new Product()
            {
                Id = id,
                Name = name,
                Price = price,
                Category = category,
                Description = description,
                ContentType = contentType,
                Unit = unit,
                UploadType = uploadType,
                TotalPage = totalPage,
                Size = size,
                ImageId = imageId,
                ImageUrl = imageUrl,
                FileId = fileId,
                FileUrl = fileUrl,
                FileDemoId = fileDemoId,
                FileDemoUrl = fileDemoUrl,
                Rating = 0,
                IsPublic = false,
                IsApproved = true,
                BookId = bookId,
                AccountId = accountId,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsDeleted = false
            };
        }
        return new Product()
        {
            Id = id,
            Name = name,
            Price = price,
            Category = category,
            Description = description,
            ContentType = contentType,
            UploadType = uploadType,
            TotalPage = totalPage,
            Size = size,
            ImageId = imageId,
            ImageUrl = imageUrl,
            FileId = fileId,
            FileUrl = fileUrl,
            FileDemoId = fileDemoId,
            FileDemoUrl = fileDemoUrl,
            Rating = 0,
            IsPublic = false,
            IsApproved = true,
            BookId = bookId,
            AccountId = accountId,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public void UpdateBook(Book book)
    {
        Book = book;
    }

    public void UpdateImageOfProducts(List<ImageOfProduct> imageOfProducts)
    {
        ImageOfProducts = imageOfProducts;
    }

    public void UpdateOrderDetails(List<OrderDetails> orderDetails)
    {
        OrderDetails = orderDetails;
    }

    public static Product CreateProduct(string name, int price, CategoryType category, string description, ContentType contentType, int unit, UploadType uploadType, int totalPage, double size, string mainImageId, string mainImageUrl, string fileId, string fileUrl, string fileDemoId, string fileDemoUrl, Guid bookId, Guid accountId)
    {
        return new Product()
        {
            Name = name,
            Price = price,
            Category = category,
            Description = description,
            ContentType = contentType,
            Unit = unit,
            UploadType = uploadType,
            TotalPage = totalPage,
            Size = size,
            ImageId = mainImageId,
            ImageUrl = mainImageUrl,
            FileId = fileId,
            FileUrl = fileUrl,
            FileDemoId = fileDemoId,
            FileDemoUrl = fileDemoUrl,
            Rating = 0,
            IsPublic = true,
            IsApproved = true,
            BookId = bookId,
            AccountId = accountId,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsDeleted = false,
        };
    }
}
