using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;

namespace EduSource.Domain.Abstraction.EntitiyFramework;

public interface IEFUnitOfWork : IAsyncDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    IAccountRepository AccountRepository { get; }
    IComboRepository ComboRepository { get; }
    IFeedbackRepository FeedbackRepository { get; }
    IImageOfProductRepository ImageOfProductRepository { get; }
    IOrderDetailsRepository OrderDetailsRepository { get; }
    IOrderRepository OrderRepository { get; }
    IProductInComboRepository ProductInComboRepository { get; }
    IProductRepository ProductRepository { get; }   
    IWishlistRepository WishlistRepository { get; }
    ICartRepository CartRepository { get; }
    IBookRepository BookRepository { get; }
    IProductRequestRepository ProductRequestRepository { get; }
}
