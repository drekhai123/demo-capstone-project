using EduSource.Domain.Abstraction.EntitiyFramework;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;

namespace EduSource.Persistence;
public class EFUnitOfWork : IEFUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public EFUnitOfWork(ApplicationDbContext context, IAccountRepository accountRepository, IComboRepository comboRepository, IFeedbackRepository feedbackRepository, IImageOfProductRepository imageOfProductRepository, IOrderDetailsRepository orderDetailsRepository, IOrderRepository orderRepository, IProductInComboRepository productInComboRepository, IProductRepository productRepository, IWishlistRepository wishlistRepository, ICartRepository cartRepository, IBookRepository bookRepository, IProductRequestRepository productRequestRepository)
    {
        _context = context;
        AccountRepository = accountRepository;
        ComboRepository = comboRepository;
        FeedbackRepository = feedbackRepository;
        ImageOfProductRepository = imageOfProductRepository;
        OrderDetailsRepository = orderDetailsRepository;
        OrderRepository = orderRepository;
        ProductInComboRepository = productInComboRepository;
        ProductRepository = productRepository;
        WishlistRepository = wishlistRepository;
        CartRepository = cartRepository;
        BookRepository = bookRepository;
        ProductRequestRepository = productRequestRepository;
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
          => await _context.DisposeAsync();

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    => await _context.SaveChangesAsync();

    public IAccountRepository AccountRepository { get; }

    public IComboRepository ComboRepository { get; }

    public IFeedbackRepository FeedbackRepository { get; }

    public IImageOfProductRepository ImageOfProductRepository { get; }

    public IOrderDetailsRepository OrderDetailsRepository { get; }

    public IOrderRepository OrderRepository { get; }

    public IProductInComboRepository ProductInComboRepository { get; }

    public IProductRepository ProductRepository { get; }

    public IWishlistRepository WishlistRepository { get; }
    public ICartRepository CartRepository { get; }
    public IBookRepository BookRepository { get; }
    public IProductRequestRepository ProductRequestRepository { get; }
}