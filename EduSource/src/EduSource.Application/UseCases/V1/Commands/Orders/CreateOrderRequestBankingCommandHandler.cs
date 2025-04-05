using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.DTOs.PaymentDTOs;
using EduSource.Contract.Services.Orders;
using EduSource.Contract.Settings;
using EduSource.Domain.Abstraction.Dappers;
using EduSource.Domain.Abstraction.EntitiyFramework;
using EduSource.Domain.Exceptions;
using Microsoft.Extensions.Options;
namespace EduSource.Application.UseCases.V1.Commands.Orders;

public sealed class CreateOrderRequestBankingCommandHandler : ICommandHandler<Command.CreateOrderRequestBankingCommand>
{
    private readonly IPaymentService _paymentService;
    private readonly PayOSSetting _payOSSetting;
    private readonly IResponseCacheService _responseCacheService;
    private readonly IEFUnitOfWork _efUnitOfWork;
    private readonly IDPUnitOfWork _dpUnitOfWork;

    public CreateOrderRequestBankingCommandHandler(IPaymentService paymentService, IOptions<PayOSSetting> payOSConfiguration, IResponseCacheService responseCacheService, IEFUnitOfWork efUnitOfWork, IDPUnitOfWork dPUnitOfWork)
    {
        _paymentService = paymentService;
        _payOSSetting = payOSConfiguration.Value;
        _responseCacheService = responseCacheService;
        _efUnitOfWork = efUnitOfWork;
        _dpUnitOfWork = dPUnitOfWork;
    }

    public async Task<Result> Handle(Command.CreateOrderRequestBankingCommand request, CancellationToken cancellationToken)
    {
        long orderId = new Random().Next(1, 100000);
        //CreateOrderBankingCommandHandler
        //Check account existed
        var accountFound = await _efUnitOfWork.AccountRepository.FindByIdAsync(request.AccountId) ?? throw new AccountException.AccountNotFoundException();

        // Create payment dto
        List<ItemDTO> itemDTOs = new()
        {
                new ItemDTO(request.Name, 1, request.Price)
        };
        var createPaymentDto = new CreatePaymentDTO(orderId, $"Cart Checkout", itemDTOs, _payOSSetting.RequestErrorUrl, _payOSSetting.RequestSuccessUrl + $"?orderId={orderId}");
        var result = await _paymentService.CreatePaymentLink(createPaymentDto);
        var resultForCache = new ResultCacheDTO.ProductRequestPaymentCacheDTO(result.OrderCode, request.AccountId, request.ProductRequestId, request.Name, request.Price, request.Name);
        // Save memory to when success or fail will know value
        await _responseCacheService.SetCacheResponseAsync($"order_{orderId}", resultForCache, TimeSpan.FromMinutes(60));

        return Result.Success(new Success<CreatePaymentResponseDTO>("", "", result));
    }
}
