using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using EduSource.Domain.Abstraction.EntitiyFramework;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Persistence.Repositories;
using EduSource.Persistance.DependencyInjection.Options;
using EduSource.Persistance.Repositories;

namespace EduSource.Persistence.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSqlConfiguration(this IServiceCollection services)
    {
        services.AddDbContextPool<DbContext, ApplicationDbContext>((provider, builder) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var options = provider.GetRequiredService<IOptionsMonitor<SqlServerRetryOptions>>();

            builder
            .EnableDetailedErrors(true)
            .EnableSensitiveDataLogging(true)
            .UseLazyLoadingProxies(true) // => If UseLazyLoadingProxies, all of the navigation fields should be VIRTUAL
            .UseSqlServer(
                connectionString: configuration.GetConnectionString("ConnectionStrings"),
                    sqlServerOptionsAction: optionsBuilder
                        => optionsBuilder
                        .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));
        });
    }

    public static OptionsBuilder<SqlServerRetryOptions> ConfigureSqlServerRetryOptions
       (this IServiceCollection services, IConfigurationSection section)
       => services
           .AddOptions<SqlServerRetryOptions>()
           .Bind(section)
           .ValidateDataAnnotations()
           .ValidateOnStart();

    public static void AddRepositoryBaseConfiguration(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(IEFUnitOfWork), typeof(EFUnitOfWork))
            .AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>))
            .AddTransient<IAccountRepository, AccountRepository>()
            .AddTransient<ICartRepository, CartRepository>()
            .AddTransient<IComboRepository, ComboRepository>()
            .AddTransient<IFeedbackRepository, FeedbackRepository>()
            .AddTransient<IImageOfProductRepository, ImageOfProductRepository>()
            .AddTransient<IOrderDetailsRepository, OrderDetailsRepository>()
            .AddTransient<IOrderRepository, OrderRepository>()
            .AddTransient<IProductInComboRepository, ProductInComboRepository>()
            .AddTransient<IProductRepository, ProductRepository>()
            .AddTransient<IWishlistRepository, WishlistRepository>()
            .AddTransient<IBookRepository, BookRepository>()
            .AddTransient<IProductRequestRepository, ProductRequestRepository>();
    }
}
