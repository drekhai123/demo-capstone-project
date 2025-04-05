using Microsoft.Extensions.DependencyInjection;
using EduSource.Domain.Abstraction.Dappers;
using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Infrastructure.Dapper.Repositories;
using EduSource.Infrastructure.Dapper;

namespace EduSource.Infrastructure.Dapper.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureDapper(this IServiceCollection services)
        => services.AddTransient<IDPUnitOfWork, DPUnitOfWork>()
                   .AddTransient<IAccountRepository, AccountRepository>()
        .AddTransient<IBookRepository, BookRepository>()
        .AddTransient<ICartRepository, CartRepository>()
        .AddTransient<IComboRepository, ComboRepository>()
        .AddTransient<IFeedbackRepository, FeedbackRepository>()
        .AddTransient<IImageOfProductRepository, ImageOfProductRepository>()
        .AddTransient<IOrderDetailsRepository, OrderDetailsRepository>()
        .AddTransient<IOrderRepository, OrderRepository>()
        .AddTransient<IProductInComboRepository, ProductInComboRepository>()
        .AddTransient<IProductRepository, ProductRepository>()
        .AddTransient<IWishlistRepository, WishlistRepository>();

}
