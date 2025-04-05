using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.DTOs.PaymentDTOs;
using EduSource.Contract.Services.Orders;
using EduSource.Contract.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
namespace EduSource.Application.UseCases.V1.Commands.Orders;

public sealed class OrderRequestFailCommandHandler : ICommandHandler<Command.OrderRequestFailCommand, Success<Response.OrderRequestFail>>
{
    private readonly IResponseCacheService _responseCacheService;
    private readonly ClientSetting _clientSetting;

    public OrderRequestFailCommandHandler
        (IResponseCacheService responseCachService,
        IOptions<ClientSetting> clientConfiguration)
    {
        _responseCacheService = responseCachService;
        _clientSetting = clientConfiguration.Value;
    }

    public async Task<Result<Success<Response.OrderRequestFail>>> Handle(Command.OrderRequestFailCommand request, CancellationToken cancellationToken)
    {
        var orderMemory = await _responseCacheService.GetCacheResponseAsync($"order_{request.OrderId}");
        var orderObject = JsonConvert.DeserializeObject<ResultCacheDTO.ProductRequestPaymentCacheDTO>(orderMemory);
        // Delete cache order
        await _responseCacheService.DeleteCacheResponseAsync($"order_{request.OrderId}");
        var result = new Response.OrderRequestFail($"{_clientSetting.Url}{_clientSetting.OrderRequest}/{orderObject.ProductRequestId}?is_success=0");
        return Result.Success(new Success<Response.OrderRequestFail>("", "", result));
    }
}
