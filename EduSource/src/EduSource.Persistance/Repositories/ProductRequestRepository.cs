using Microsoft.EntityFrameworkCore;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Domain.Entities;
using EduSource.Persistence;
using EduSource.Persistence.Repositories;

namespace EduSource.Persistance.Repositories;

public class ProductRequestRepository(ApplicationDbContext context) : RepositoryBase<ProductRequest, int>(context), IProductRequestRepository
{
    private readonly ApplicationDbContext _context = context;
}
