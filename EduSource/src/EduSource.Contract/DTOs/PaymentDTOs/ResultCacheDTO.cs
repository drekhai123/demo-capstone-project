namespace EduSource.Contract.DTOs.PaymentDTOs;

public sealed class ResultCacheDTO
{
    public class ProductPaymentCacheDTO
    {
        public ProductPaymentCacheDTO(long orderCode, Guid accountId, string description, List<Guid> productIds, bool isFromCart)
        {
            OrderCode = orderCode;
            AccountId = accountId;
            Description = description;
            ProductIds = productIds;
            IsFromCart = isFromCart;
        }

        public long OrderCode { get; set; }
        public Guid AccountId { get; set; }
        public string Description { get; set; }
        public List<Guid> ProductIds { get; set; }
        public bool IsFromCart { get; set; }
    }

    public class ProductRequestPaymentCacheDTO
    {
        public ProductRequestPaymentCacheDTO(long orderCode, Guid accountId, int productRequestId, string description, int price, string name)
        {
            OrderCode = orderCode;
            AccountId = accountId;
            ProductRequestId = productRequestId;
            Description = description;
            Price = price;
            Name = name;
        }

        public long OrderCode { get; set; }
        public Guid AccountId { get; set; }
        public int ProductRequestId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }

}
