using EduSource.Domain.Abstraction.Entities;

namespace EduSource.Domain.Entities;

public class OrderDetails : DomainEntity<Guid>
{
    public OrderDetails()
    {

    }

    public int Quantity { get; private set; }
    public Guid? OrderId { get; private set; }
    public virtual Order? Order { get; private set; }
    public Guid? ComboId { get; private set; }
    public virtual Combo Combo { get; private set; }
    public Guid? ProductId { get; private set; }
    public virtual Product? Product { get; private set; }
    public int? ProductRequestId { get; private set; }
    public virtual ProductRequest ProductRequest { get; private set; }

    public static OrderDetails CreateOrderDetailsWithProduct(int quantity, Guid orderId, Guid productId)
    {
        return new OrderDetails()
        {
            Quantity = quantity,
            OrderId = orderId,
            ProductId = productId,
            IsDeleted = false
        };
    }

    public static OrderDetails CreateOrderDetailsWithProductRequest(int quantity, Guid orderId, int productRequestId)
    {
        return new OrderDetails()
        {
            Quantity = quantity,
            OrderId = orderId,
            ProductRequestId = productRequestId,
            IsDeleted = false
        };
    }

    public void UpdateOrder(Order order)
    {
        Order = order;
    }

    public void UpdateProduct(Product product)
    {
        Product = product;
    }

    public void UpdateProductRequest(ProductRequest productRequest)
    {
        ProductRequest = productRequest;
    }
}
