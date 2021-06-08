using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Helpers;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {

            var acct = new Account
            {
                Cloud = config.Value.CloudName,
                ApiKey = config.Value.ApiKey,
                ApiSecret = config.Value.ApiSecret


            };
            _cloudinary = new Cloudinary(acct);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {

                using var stream = file.OpenReadStream();

                var uploadParms = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")

                };
                uploadResult = await _cloudinary.UploadAsync(uploadParms);

            }

            return uploadResult;


        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
           var deleteParms = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParms);

            return result;
        }
    }
}