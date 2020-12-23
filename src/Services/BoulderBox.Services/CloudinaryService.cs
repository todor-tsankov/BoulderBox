using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.ValidationAttributes;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;

namespace BoulderBox.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Account account;

        public CloudinaryService(string cloudName, string apiKey, string apiSecret)
        {
            this.account = new Account(cloudName, apiKey, apiSecret);
        }

        public async Task<ImageInputModel> SaveImageAsync(IFormFile formFile)
        {
            var isValid = new ImageAttribute().IsValid(formFile);

            if (formFile == null || !isValid)
            {
                return null;
            }

            var guid = Guid.NewGuid().ToString();
            var extension = await Image.DetectFormatAsync(formFile.OpenReadStream());

            var fileName = $"{guid}.{extension.Name}";
            var cloudinary = new Cloudinary(this.account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, formFile.OpenReadStream()),
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            var imageInput = new ImageInputModel()
            {
                Source = uploadResult.SecureUrl.AbsoluteUri,
            };

            return imageInput;
        }
    }
}
