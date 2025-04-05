using Microsoft.EntityFrameworkCore;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Domain.Entities;
using EduSource.Persistence;
using EduSource.Persistence.Repositories;

namespace EduSource.Persistance.Repositories;

public class ImageOfProductRepository(ApplicationDbContext context) : RepositoryBase<ImageOfProduct, Guid>(context), IImageOfProductRepository
{
    private readonly ApplicationDbContext _context = context;
}
