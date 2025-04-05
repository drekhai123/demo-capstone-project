using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EduSource.Contract.DTOs.MediaDTOs;

namespace EduSource.Infrastructure.Services;

public class MediaService : IMediaService
{
    private readonly CloudinarySetting _cloudinarySetting;
    private readonly Cloudinary _cloudinary;
    public MediaService(IOptions<CloudinarySetting> cloudinaryConfig)
    {
        var account = new Account(cloudinaryConfig.Value.CloudName,
            cloudinaryConfig.Value.ApiKey,
            cloudinaryConfig.Value.ApiSecret);

        _cloudinary = new Cloudinary(account);
        _cloudinarySetting = cloudinaryConfig.Value;
    }

    public async Task<bool> DeleteFileAsync(string publicId)
    {
        var deletionParams = new DeletionParams(publicId);
        var isCheck = await _cloudinary.DestroyAsync(deletionParams);
        if (isCheck.Error != null) return false;
        return true;
    }

    public async Task<ImageDTO> UploadImageAsync(string fileName, IFormFile fileImage)
    {
        var isImage = fileImage.ContentType.StartsWith("image");
        var fileExtension = Path.GetExtension(fileName); // Get the file extension
        var fileBaseName = Path.GetFileNameWithoutExtension(fileName); // Get filename without extension

        UploadResult uploadResult;

        if (isImage)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileImage.OpenReadStream()),
                Folder = _cloudinarySetting.Folder
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        else
        {
            var safeFileName = $"{fileBaseName.Replace(" ", "-")}{fileExtension}"; // Replace spaces with hyphens
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, fileImage.OpenReadStream()),
                Folder = _cloudinarySetting.Folder,
                PublicId = safeFileName
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        if (uploadResult?.StatusCode != System.Net.HttpStatusCode.OK) return null;

        // Construct the correct download URL with the file extension
        var fileUrl = $"{uploadResult.SecureUri.AbsoluteUri}";

        return new ImageDTO
        {
            ImageUrl = fileUrl,
            PublicImageId = uploadResult.PublicId
        };
    }




    public async Task<List<ImageDTO>> UploadImagesAsync(List<IFormFile> fileImages)
    {
        var imageDtoList = new List<ImageDTO>();

        foreach (var fileImage in fileImages)
        {
            var fileName = fileImage.FileName;

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileImage.OpenReadStream()),
                Folder = _cloudinarySetting.Folder,
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult?.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var imageUrl = uploadResult.Url.AbsoluteUri;
                var imageId = uploadResult.PublicId;

                imageDtoList.Add(new ImageDTO
                {
                    ImageUrl = imageUrl,
                    PublicImageId = imageId
                });
            }
        }

        return imageDtoList;
    }
}
