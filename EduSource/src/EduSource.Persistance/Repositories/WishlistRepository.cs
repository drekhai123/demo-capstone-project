using Microsoft.EntityFrameworkCore;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Domain.Entities;
using EduSource.Persistence;
using EduSource.Persistence.Repositories;

namespace EduSource.Persistance.Repositories;

public class WishlistRepository(ApplicationDbContext context) : RepositoryBase<Wishlist, Guid>(context), IWishlistRepository
{
    private readonly ApplicationDbContext _context = context;
}
