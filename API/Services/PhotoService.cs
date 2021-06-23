using System.Reflection;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudiary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
                var acc = new Account
                (
                    config.Value.CloudName,
                    config.Value.ApiKey,
                    config.Value.ApiSecret
                );
                _cloudiary = new Cloudinary (acc);
        }

       async Task<ImageUploadResult> IPhotoService.AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName,stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudiary.UploadAsync(uploadParams);

            }
            return uploadResult;
        }

        async Task<DeletionResult> IPhotoService.DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudiary.DestroyAsync(deleteParams);

            return result;
        }
    }
}