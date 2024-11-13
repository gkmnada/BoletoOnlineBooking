using Catalog.Application.Interfaces.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Catalog.Persistence.Services
{
    public class FileService : IFileService
    {
        private readonly Cloudinary _cloudinary;
        private readonly Account _account;
        private readonly IConfiguration _configuration;

        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
            _account = new Account(_configuration.GetValue<string>("Cloudinary:CloudName"), _configuration.GetValue<string>("Cloudinary:ApiKey"), _configuration.GetValue<string>("Cloudinary:ApiSecret"));
            _cloudinary = new Cloudinary(_account);
            _cloudinary.Api.Client.Timeout = TimeSpan.FromMinutes(10);
        }

        public async Task<bool> DeleteFileAsync(string publicID)
        {
            publicID = publicID.Substring(publicID.IndexOf("v1/") + "v1/".Length);

            var deleteParams = new DeletionParams(publicID);
            var deleteResult = await _cloudinary.DestroyAsync(deleteParams);

            return deleteResult.Result == "ok";
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "boleto-images"
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
                string publicID = _cloudinary.Api.UrlImgUp.BuildUrl(uploadResult.PublicId);

                return publicID;
            }

            return "";
        }

        public async Task<string> UploadVideoAsync(IFormFile file)
        {
            var uploadResult = new VideoUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "boleto-videos"
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
                string publicID = _cloudinary.Api.UrlVideoUp.BuildUrl(uploadResult.PublicId);

                return publicID;
            }
            return "";
        }
    }
}
