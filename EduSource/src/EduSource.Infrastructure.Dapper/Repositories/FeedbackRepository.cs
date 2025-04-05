using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace EduSource.Infrastructure.Dapper.Repositories;

public class FeedbackRepository : IFeedbackRepository
{
    private readonly IConfiguration _configuration;
    public FeedbackRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<int> AddAsync(Feedback entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(Feedback entity)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Feedback>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Feedback>? GetByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(Feedback entity)
    {
        throw new NotImplementedException();
    }
}
