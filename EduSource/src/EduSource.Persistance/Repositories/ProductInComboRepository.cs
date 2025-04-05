using Microsoft.EntityFrameworkCore;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Domain.Entities;
using EduSource.Persistence;
using EduSource.Persistence.Repositories;

namespace EduSource.Persistance.Repositories;

public class ProductInComboRepository(ApplicationDbContext context) : RepositoryBase<ProductInCombo, Guid>(context), IProductInComboRepository
{
    private readonly ApplicationDbContext _context = context;
}
