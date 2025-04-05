using Microsoft.EntityFrameworkCore;
using EduSource.Domain.Entities;

namespace EduSource.Persistence;
public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

        // Configure ProductRequest.Id to not be an identity column
        builder.Entity<ProductRequest>()
            .Property(p => p.Id)
            .ValueGeneratedNever(); // Prevents Identity column behavior

        base.OnModelCreating(builder);
    }


    public DbSet<Account> Accounts { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Combo> Combos { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<ImageOfProduct> ImageOfProducts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<ProductInCombo> ProductInCombos { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<ProductRequest> ProductRequests { get; set; }


}