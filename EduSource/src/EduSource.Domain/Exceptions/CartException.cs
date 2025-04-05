using EduSource.Contract.Enumarations.MessagesList;

namespace EduSource.Domain.Exceptions;

public static class CartException
{
    public sealed class ProductHasAlreadyAddedToCartException : BadRequestException
    {
        public ProductHasAlreadyAddedToCartException() : base(MessagesList.CartProductHasAlreadyAddedToCartException.GetMessage().Message, MessagesList.CartProductHasAlreadyAddedToCartException.GetMessage().Code)
        {

        }
    }

    public sealed class ProductNotFoundInCartException : NotFoundException
    {
        public ProductNotFoundInCartException() : base(MessagesList.CartProductNotFoundInCartException.GetMessage().Message, MessagesList.CartProductNotFoundInCartException.GetMessage().Code)
        {

        }
    }

    public sealed class ProductNotInCartException : NotFoundException
    {
        public ProductNotInCartException() : base(MessagesList.CartProductNotInCartException.GetMessage().Message, MessagesList.CartProductNotInCartException.GetMessage().Code)
        {

        }
    }
}
