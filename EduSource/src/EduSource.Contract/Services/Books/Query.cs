using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using static EduSource.Contract.Services.Books.Filter;

namespace EduSource.Contract.Services.Books;

public static class Query
{
    public record GetAllBooksQuery(int PageIndex,
            int PageSize,
            BookFilter FilterParams,
            string[] SelectedColumns) : IQuery<Success<PagedResult<Response.BookResponse>>>;
}
