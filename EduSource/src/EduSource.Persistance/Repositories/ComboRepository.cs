using Microsoft.EntityFrameworkCore;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Domain.Entities;
using EduSource.Persistence;
using EduSource.Persistence.Repositories;

namespace EduSource.Persistance.Repositories;

public class ComboRepository(ApplicationDbContext context) : RepositoryBase<Combo, Guid>(context), IComboRepository
{
    private readonly ApplicationDbContext _context = context;
}
