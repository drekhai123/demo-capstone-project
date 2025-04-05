using EduSource.Contract.Enumarations.Book;

namespace EduSource.Contract.Services.Books;

public static class Filter
{
    public record BookFilter(Guid? Id, string? Name, int? GradeLevel, CategoryType? Category);
}