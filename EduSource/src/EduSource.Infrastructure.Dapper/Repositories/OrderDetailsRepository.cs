using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace EduSource.Infrastructure.Dapper.Repositories;

public class OrderDetailsRepository : IOrderDetailsRepository
{
    private readonly IConfiguration _configuration;
    public OrderDetailsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<int> AddAsync(OrderDetails entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(OrderDetails entity)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<OrderDetails>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<OrderDetails>? GetByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(OrderDetails entity)
    {
        throw new NotImplementedException();
    }
}
