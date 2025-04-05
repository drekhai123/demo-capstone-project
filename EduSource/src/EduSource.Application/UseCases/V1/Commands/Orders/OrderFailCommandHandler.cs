using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Services.Orders;
using EduSource.Contract.Settings;
using Microsoft.Extensions.Options;
namespace EduSource.Application.UseCases.V1.Commands.Orders;

public sealed class OrderFailCommandHandler : ICommandHandler<Command.OrderFailCommand, Success<Response.OrderFail>>
{
    private readonly IResponseCacheService _responseCacheService;
    private readonly ClientSetting _clientSetting;

    public OrderFailCommandHandler
        (IResponseCacheService responseCachService,
        IOptions<ClientSetting> clientConfiguration)
    {
        _responseCacheService = responseCachService;
        _clientSetting = clientConfiguration.Value;
    }

    public async Task<Result<Success<Response.OrderFail>>> Handle(Command.OrderFailCommand request, CancellationToken cancellationToken)
    {
        // Delete cache order
        await _responseCacheService.DeleteCacheResponseAsync($"order_{request.OrderId}");
        var result = new Response.OrderFail($"{_clientSetting.Url}{_clientSetting.OrderFail}");
        return Result.Success(new Success<Response.OrderFail>("", "", result));
    }
}
