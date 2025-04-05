using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Entities;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Services.Books;
using System.Text;

namespace EduSource.Infrastructure.Dapper.Repositories;
public class BookRepository : IBookRepository
{
    private readonly IConfiguration _configuration;
    public BookRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<int> AddAsync(Book entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(Book entity)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResult<Book>> GetPagedAsync(int pageIndex, int pageSize, Filter.BookFilter filterParams, string[] selectedColumns)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            // Valid columns for selecting
            var validColumns = new HashSet<string> { "Id", "Name", "ImageUrl", "GradeLevel", "Category"};
            var columns = selectedColumns?.Where(c => validColumns.Contains(c)).ToArray();

            // If no selected columns, select all
            var selectedColumnsString = columns?.Length > 0 ? string.Join(", ", columns) : "*";

            // Start building the query
            var queryBuilder = new StringBuilder($"SELECT {selectedColumnsString} FROM Books WHERE 1=1 AND IsDeleted = 0");

            var parameters = new DynamicParameters();

            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 10 : pageSize > 100 ? 100 : pageSize;

            var totalCountQuery = new StringBuilder($@"
        SELECT COUNT(1) 
        FROM Books s
        WHERE 1=1 AND IsDeleted = 0");

            // Filter by Id
            if (filterParams?.Id.HasValue == true)
            {
                queryBuilder.Append(" AND Id = @Id");
                totalCountQuery.Append(" AND Id = @Id");
                parameters.Add("Id", $"{filterParams.Id}");
            }

            // Filter by Name
            if (!string.IsNullOrEmpty(filterParams?.Name))
            {
                queryBuilder.Append(" AND Name LIKE @Name");
                totalCountQuery.Append(" AND Name LIKE @Name");
                parameters.Add("Name", $"%{filterParams.Name}%");
            }

            //Filter by GradeLevel
            if (filterParams?.GradeLevel.HasValue == true)
            {
                queryBuilder.Append(" AND GradeLevel = @GradeLevel");
                totalCountQuery.Append(" AND GradeLevel = @GradeLevel");
                parameters.Add("GradeLevel", $"{filterParams.GradeLevel}");
            }

            //Filter by GradeLevel
            if (filterParams?.GradeLevel.HasValue == true)
            {
                queryBuilder.Append(" AND GradeLevel = @GradeLevel");
                totalCountQuery.Append(" AND GradeLevel = @GradeLevel");
                parameters.Add("GradeLevel", $"{filterParams.GradeLevel}");
            }

            //Filter by CategoryType
            if (filterParams?.Category.HasValue == true)
            {
                queryBuilder.Append(" AND Category = @Category");
                totalCountQuery.Append(" AND Category = @Category");
                parameters.Add("Category", $"{(int)filterParams.Category}");
            }

            //Count TotalCount
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountQuery.ToString(), parameters);

            //Count TotalPages
            var totalPages = Math.Ceiling((totalCount / (double)pageSize));

            var offset = (pageIndex - 1) * pageSize;
            var paginatedQuery = $"{queryBuilder} ORDER BY Category ASC, GradeLevel ASC OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            var items = (await connection.QueryAsync<Book>(paginatedQuery, parameters)).ToList();

            return new PagedResult<Book>(items, pageIndex, pageSize, totalCount, totalPages);

        }
    }

    public Task<int> UpdateAsync(Book entity)
    {
        throw new NotImplementedException();
    }

    Task<IReadOnlyCollection<Book>> IGenericRepository<Book>.GetAllAsync()
    {
        throw new NotImplementedException();
    }

    Task<Book>? IGenericRepository<Book>.GetByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }
}
