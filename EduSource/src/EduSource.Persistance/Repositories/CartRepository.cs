using Microsoft.EntityFrameworkCore;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Domain.Entities;
using EduSource.Persistence;
using EduSource.Persistence.Repositories;

namespace EduSource.Persistance.Repositories;

public class CartRepository(ApplicationDbContext context) : RepositoryBase<Cart, Guid>(context), ICartRepository
{
    private readonly ApplicationDbContext _context = context;
}
