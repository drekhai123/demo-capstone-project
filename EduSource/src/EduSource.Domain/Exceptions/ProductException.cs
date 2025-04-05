using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Domain.Exceptions;
namespace EduSource.Domain.Exceptions;

public static class ProductException
{
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException()
            : base(MessagesList.ProductNotFoundException.GetMessage().Message,
                   MessagesList.ProductNotFoundException.GetMessage().Code)
        { }
    }
}
