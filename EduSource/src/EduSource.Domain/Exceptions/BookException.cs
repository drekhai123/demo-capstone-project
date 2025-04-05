using EduSource.Contract.Enumarations.MessagesList;

namespace EduSource.Domain.Exceptions;

public static class BookException
{
    public sealed class BookNotFoundException : NotFoundException
    {
        public BookNotFoundException()
            : base(MessagesList.BookNotFoundException.GetMessage().Message,
                   MessagesList.BookNotFoundException.GetMessage().Code)
        { }
    }
}
