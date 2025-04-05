using Microsoft.AspNetCore.Http;
using EduSource.Contract.DTOs.MediaDTOs;

namespace EduSource.Contract.Abstractions.Services;

public interface IMediaService
{
    Task<ImageDTO> UploadImageAsync(string fileName, IFormFile fileImage);
    Task<List<ImageDTO>> UploadImagesAsync(List<IFormFile> fileImages);
    Task<bool> DeleteFileAsync(string publicId);
}
