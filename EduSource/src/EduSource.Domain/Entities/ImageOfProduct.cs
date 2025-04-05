using EduSource.Domain.Abstraction.Entities;

namespace EduSource.Domain.Entities;

public class ImageOfProduct : DomainEntity<Guid>
{
    public ImageOfProduct()
    {

    }

    public string ImageId { get; private set; }
    public string ImageUrl { get; private set; }
    public Guid ProductId { get; private set; }
    public virtual Product Product { get; private set; }

    public static ImageOfProduct CreateImageOfProductForSeedData(string imageId, string imageUrl, Guid productId)
    {
        return new ImageOfProduct()
        {
            ImageId = imageId,
            ImageUrl = imageUrl,
            ProductId = productId,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public static ImageOfProduct CreateImageOfProduct(string imageId, string imageUrl, Guid productId)
    {
        return new ImageOfProduct()
        {
            ImageId = imageId,
            ImageUrl = imageUrl,
            ProductId = productId,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsDeleted = false
        };
    }
}
