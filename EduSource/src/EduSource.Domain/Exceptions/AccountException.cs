using EduSource.Contract.Enumarations.MessagesList;

namespace EduSource.Domain.Exceptions;

public static class AccountException
{
    public sealed class AccountNotFoundException : NotFoundException
    {
        public AccountNotFoundException()
            : base(MessagesList.AccountNotFoundException.GetMessage().Message,
                   MessagesList.AccountNotFoundException.GetMessage().Code)
        { }
    }
    public sealed class AccountUpdateEmailExit : NotFoundException
    {
        public AccountUpdateEmailExit()
        : base(MessagesList.AccountEmailUpdateExit.GetMessage().Message,
               MessagesList.AccountEmailUpdateExit.GetMessage().Code)
        { }
    }

    public sealed class AccountEmailDuplicateException : BadRequestException
    {
        public AccountEmailDuplicateException()
      : base(MessagesList.AccountEmailDuplicate.GetMessage().Message,
             MessagesList.AccountEmailDuplicate.GetMessage().Code)
        { }
    }

    public sealed class AccountNotLoginLocalException : BadRequestException
    {
        public AccountNotLoginLocalException()
      : base(MessagesList.AccountNotLoginUpdate.GetMessage().Message,
             MessagesList.AccountNotLoginUpdate.GetMessage().Code)
        { }
    }
}
