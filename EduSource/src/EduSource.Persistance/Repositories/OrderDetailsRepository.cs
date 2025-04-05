using Microsoft.EntityFrameworkCore;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Domain.Entities;
using EduSource.Persistence;
using EduSource.Persistence.Repositories;

namespace EduSource.Persistance.Repositories;

public class OrderDetailsRepository(ApplicationDbContext context) : RepositoryBase<OrderDetails, Guid>(context), IOrderDetailsRepository
{
    private readonly ApplicationDbContext _context = context;
}
