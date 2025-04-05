using EduSource.Domain.Abstraction.Dappers;
using EduSource.Domain.Abstraction.Dappers.Repositories;

namespace EduSource.Infrastructure.Dapper;
public class DPUnitOfWork : IDPUnitOfWork
{
    public DPUnitOfWork(IAccountRepository accountRepository, IComboRepository comboRepository, IFeedbackRepository feedbackRepository, IImageOfProductRepository imageOfProductRepository, IOrderDetailsRepository orderDetailsRepository, IOrderRepository orderRepository, IProductInComboRepository productInComboRepository, IProductRepository productRepository, IWishlistRepository wishlistRepository, ICartRepository cartRepository, IBookRepository bookRepository)
    {
        AccountRepositories = accountRepository;
        ComboRepositories = comboRepository;
        FeedbackRepositories = feedbackRepository;
        ImageOfProductRepositories = imageOfProductRepository;
        OrderDetailsRepositories = orderDetailsRepository;
        OrderRepositories = orderRepository;
        ProductInComboRepositories = productInComboRepository;
        ProductRepositories = productRepository;
        WishlistRepositories = wishlistRepository;
        CartRepositories = cartRepository;
        BookRepositories = bookRepository;
    }
    public IAccountRepository AccountRepositories { get; }

    public IComboRepository ComboRepositories { get; }

    public IFeedbackRepository FeedbackRepositories { get; }

    public IImageOfProductRepository ImageOfProductRepositories { get; }

    public IOrderDetailsRepository OrderDetailsRepositories { get; }

    public IOrderRepository OrderRepositories { get; }

    public IProductInComboRepository ProductInComboRepositories { get; }

    public IProductRepository ProductRepositories { get; }

    public IWishlistRepository WishlistRepositories { get; }

    public ICartRepository CartRepositories { get; }

    public IBookRepository BookRepositories {  get; }
}
