using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace EduSource.Infrastructure.Dapper.Repositories;

public class ComboRepository : IComboRepository
{
    private readonly IConfiguration _configuration;
    public ComboRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<int> AddAsync(Combo entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(Combo entity)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Combo>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Combo>? GetByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(Combo entity)
    {
        throw new NotImplementedException();
    }
}
