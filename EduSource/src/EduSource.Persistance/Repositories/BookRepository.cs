using Microsoft.EntityFrameworkCore;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Domain.Entities;
using EduSource.Persistence;
using EduSource.Persistence.Repositories;

namespace EduSource.Persistance.Repositories;

public class BookRepository(ApplicationDbContext context) : RepositoryBase<Book, Guid>(context), IBookRepository
{
    private readonly ApplicationDbContext _context = context;
}
