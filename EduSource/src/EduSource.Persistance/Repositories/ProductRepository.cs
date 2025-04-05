using Microsoft.EntityFrameworkCore;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Domain.Entities;
using EduSource.Persistence;
using EduSource.Persistence.Repositories;

namespace EduSource.Persistance.Repositories;

public class ProductRepository(ApplicationDbContext context) : RepositoryBase<Product, Guid>(context), IProductRepository
{
    private readonly ApplicationDbContext _context = context;
}
