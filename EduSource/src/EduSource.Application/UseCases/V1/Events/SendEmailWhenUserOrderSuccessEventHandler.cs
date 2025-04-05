using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Services.Orders;
using EduSource.Contract.Settings;
using Microsoft.Extensions.Options;
using System.Text;

namespace EduSource.Application.UseCases.V1.Events;

public sealed class SendEmailWhenUserOrderSuccessEventHandler
    : IDomainEventHandler<DomainEvent.NotiUserOrderSuccess>
{
    private readonly IEmailService _emailService;
    private readonly ClientSetting _clientSetting;

    public SendEmailWhenUserOrderSuccessEventHandler(IEmailService emailService,
        IOptions<ClientSetting> clientConfig)
    {
        _emailService = emailService;
        _clientSetting = clientConfig.Value;
    }

    public async Task Handle(DomainEvent.NotiUserOrderSuccess notification, CancellationToken cancellationToken)
    {
        // 🔹 Convert List to HTML Table Rows
        var invoiceItemsHtml = new StringBuilder();
        foreach (var item in notification.InvoiceItems)
        {
            invoiceItemsHtml.AppendFormat(
                "<tr><td>{0}</td><td>{1}</td><td>{2} VND</td><td>{3} VND</td></tr>",
                item.Name, item.Quantity, item.Price, item.Quantity * item.Price);
        }
        await _emailService.SendMailAsync
                    (notification.Email,
                    "Invoice",
                    "EmailOrderSuccess.html", new Dictionary<string, string> {
            {"ToEmail", notification.Email},
                        {"InvoiceNumber", notification.InvoiceNumber },
                        {"InvoiceDate", notification.InvoiceDate },
                        {"InvoiceItems", invoiceItemsHtml.ToString()},
                        {"TotalAmount", notification.TotalAmount.ToString() + " VND" }
               
                });
    }
}
