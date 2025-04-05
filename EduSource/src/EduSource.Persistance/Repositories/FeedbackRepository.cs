using Microsoft.EntityFrameworkCore;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Domain.Entities;
using EduSource.Persistence;
using EduSource.Persistence.Repositories;

namespace EduSource.Persistance.Repositories;

public class FeedbackRepository(ApplicationDbContext context) : RepositoryBase<Feedback, Guid>(context), IFeedbackRepository
{
    private readonly ApplicationDbContext _context = context;
}
