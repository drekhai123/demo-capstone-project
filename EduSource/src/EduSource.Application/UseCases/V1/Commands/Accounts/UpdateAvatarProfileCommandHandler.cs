using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.DTOs.Account;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Accounts;
using EduSource.Domain.Abstraction.EntitiyFramework;
using Microsoft.AspNetCore.Http;
using static EduSource.Domain.Exceptions.AccountException;

namespace EduSource.Application.UseCases.V1.Commands.Account;

public sealed class UpdateAvatarProfileCommandHandler : ICommandHandler<Command.UpdateAvatarCommand, Success<AccountAvatarDto>>
{
    private readonly IEFUnitOfWork _efUnitOfWork;
    private readonly IMediaService _mediaService;

    public UpdateAvatarProfileCommandHandler
        (IEFUnitOfWork efUnitOfWork,
        IMediaService mediaService)
    {
        _efUnitOfWork = efUnitOfWork;
        _mediaService = mediaService;
    }

    public async Task<Result<Success<AccountAvatarDto>>> Handle(Command.UpdateAvatarCommand request, CancellationToken cancellationToken)
    {
        var account = await _efUnitOfWork.AccountRepository.FindByIdAsync(request.UserId);
        if (account == null) throw new AccountNotFoundException();
        if((!string.IsNullOrEmpty(account.CropAvatarId) && !string.IsNullOrEmpty(account.FullAvatarId)) == true)
        {
            var oldCropPublicId = account.CropAvatarId;
            var oldFullPublicId = account.FullAvatarId;
            await Task.WhenAll(
                _mediaService.DeleteFileAsync(oldCropPublicId),
                _mediaService.DeleteFileAsync(oldFullPublicId)
            );
        }

        var uploadImages = await _mediaService.UploadImagesAsync(new List<IFormFile> { request.CropAvatarFile, request.FullAvatarFile });

        account.UpdateAvatarProfileUser(uploadImages[0].ImageUrl, uploadImages[0].PublicImageId, uploadImages[1].ImageUrl, uploadImages[1].PublicImageId);
        await _efUnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new Success<AccountAvatarDto>
            (MessagesList.AccountUploadAvatarSuccess.GetMessage().Code,
            MessagesList.AccountUploadAvatarSuccess.GetMessage().Message,
            new AccountAvatarDto
            {
                CropAvatarLink = uploadImages[0].ImageUrl,
                FullAvatarLink = uploadImages[1].ImageUrl,
            }));
    }
}
