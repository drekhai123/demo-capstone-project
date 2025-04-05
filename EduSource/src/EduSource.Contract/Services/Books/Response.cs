using EduSource.Contract.Enumarations.Book;

namespace EduSource.Contract.Services.Books;

public static class Response
{
    public record BookResponse(Guid Id, string Name, string ImageUrl, int GradeLevel, CategoryType Category);
}
