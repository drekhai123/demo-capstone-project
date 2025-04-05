using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Entities;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Services.Accounts;
using System.Text;

namespace EduSource.Infrastructure.Dapper.Repositories;
public class AccountRepository : IAccountRepository
{
    private readonly IConfiguration _configuration;
    public AccountRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool>? AccountExistSystemAsync(Guid userId)
    {
        var sql = "SELECT CASE WHEN EXISTS (SELECT 1 FROM Accounts WHERE Id = @Id) THEN 1 ELSE 0 END";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.ExecuteScalarAsync<bool>(sql, new { Id = userId });
            return result;
        }
    }

    public Task<int> AddAsync(Account entity)
    {
        throw new NotImplementedException();
    }

    public async Task<int> CountAllUsers()
    {
        var sql = "SELECT COUNT(*) FROM Accounts WHERE RoleId = 2";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.ExecuteScalarAsync<int>(sql);
            return result;
        }
    }

    public Task<int> DeleteAsync(Account entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> EmailExistSystemAsync(string email)
    {
        var sql = "SELECT CASE WHEN EXISTS (SELECT 1 FROM Accounts WHERE Email = @Email) THEN 1 ELSE 0 END";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.ExecuteScalarAsync<bool>(sql, new { Email = email });
            return result;
        }
    }

    public Task<IReadOnlyCollection<Account>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Account> GetByEmailAsync(string email)
    {
        var sql = "SELECT Id, FirstName, LastName, Email, PhoneNumber, Password, RoleId, LoginType, CropAvatarUrl, FullAvatarUrl, IsDeleted FROM Accounts WHERE Email = @Email";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.QuerySingleOrDefaultAsync<Account>(sql, new { Email = email });
            return result;
        }
    }

    public async Task<Account> GetByIdAsync(Guid id)
    {
        var sql = @"
        SELECT 
        [Id]
      ,[FirstName]
      ,[LastName]
      ,[Email]
      ,[PhoneNumber]
      ,[Password]
      ,[Biography]
      ,[CropAvatarUrl]
      ,[CropAvatarId]
      ,[FullAvatarUrl]
      ,[FullAvatarId]
      ,[CropCoverPhotoId]
      ,[CropCoverPhotoUrl]
      ,[FullCoverPhotoId]
      ,[FullCoverPhotoUrl]
      ,[LoginType]
      ,[GenderType]
      ,[RoleId]
      ,[CreatedDate]
      ,[ModifiedDate]
      ,[IsDeleted]
        FROM [Accounts]
        WHERE [Id] = @id";
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            await connection.OpenAsync();
            var result = await connection.QuerySingleOrDefaultAsync<Account>(sql, new { Id = id });
            return result;
        }
    }

    public async Task<PagedResult<Account>> GetPagedAsync(int pageIndex, int pageSize, Filter.AccountFilter filterParams, string[] selectedColumns)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings")))
        {
            // Valid columns for selecting
            var validColumns = new HashSet<string> { "Id", "FirstName", "LastName", "Email", "PhoneNumber", "Password", "RoleId", "GenderType", "LoginType", "IsDeleted", "CropAvatarUrl", "FullAvatarUrl", "CropCoverPhotoUrl", "FullCoverPhotoUrl", "Biography" };
            var columns = selectedColumns?.Where(c => validColumns.Contains(c)).ToArray();

            // If no selected columns, select all
            var selectedColumnsString = columns?.Length > 0 ? string.Join(", ", columns) : "*";

            // Start building the query
            var queryBuilder = new StringBuilder($"SELECT {selectedColumnsString} FROM Accounts WHERE 1=1");

            var parameters = new DynamicParameters();

            // Filter by Id
            if (filterParams?.Id.HasValue == true)
            {
                queryBuilder.Append(" AND Id LIKE @Id");
                parameters.Add("Id", $"%{filterParams.Id}%");
            }

            // Filter by FirstName
            if (!string.IsNullOrEmpty(filterParams?.FirstName))
            {
                queryBuilder.Append(" AND FirstName LIKE @FirstName");
                parameters.Add("FirstName", $"%{filterParams.FirstName}%");
            }

            // Filter by IsDeleted (e.g., true/false)
            if (filterParams?.IsDeleted.HasValue == true)
            {
                queryBuilder.Append(" AND IsDeleted = @IsDeleted");
                parameters.Add("IsDeleted", filterParams.IsDeleted.Value);
            }

            if (filterParams?.RoleType.HasValue == true)
            {
                queryBuilder.Append(" AND RoleId = @RoleId");
                parameters.Add("RoleId", (int)filterParams.RoleType.Value);  // Cast RoleType enum to its integer value
            }

            return await PagedResult<Account>.CreateAsync(connection, queryBuilder.ToString(), parameters, pageIndex, pageSize);
        }
    }

    public Task<int> UpdateAsync(Account entity)
    {
        throw new NotImplementedException();
    }
}
