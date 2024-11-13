using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<string> UploadVideoAsync(IFormFile file);
        Task<bool> DeleteFileAsync(string publicID);
    }
}
