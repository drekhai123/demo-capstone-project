using EduSource.Domain.Abstraction.Dappers.Repositories;

namespace EduSource.Domain.Abstraction.Dappers;

public interface IDPUnitOfWork
{
    IAccountRepository AccountRepositories { get; }
    IComboRepository ComboRepositories { get; }
    IFeedbackRepository FeedbackRepositories { get; }
    IImageOfProductRepository ImageOfProductRepositories { get; }
    IOrderDetailsRepository OrderDetailsRepositories { get; }
    IOrderRepository OrderRepositories { get; }
    IProductInComboRepository ProductInComboRepositories { get; }
    IProductRepository ProductRepositories { get; }
    IWishlistRepository WishlistRepositories { get; }
    ICartRepository CartRepositories { get; }
    IBookRepository BookRepositories { get; }
}
