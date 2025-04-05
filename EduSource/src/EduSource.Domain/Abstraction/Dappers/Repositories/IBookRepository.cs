using EduSource.Contract.Abstractions.Shared;
using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Entities;
using static EduSource.Contract.Services.Books.Filter;

namespace EduSource.Domain.Abstraction.Dappers.Repositories;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<PagedResult<Book>> GetPagedAsync(int pageIndex, int pageSize, BookFilter filterParams, string[] selectedColumns);

}
