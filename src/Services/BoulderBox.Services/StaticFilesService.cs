using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;

namespace BoulderBox.Services
{
    public class StaticFilesService : IStaticFilesService
    {
        private readonly IHostEnvironment environment;

        public StaticFilesService(IHostEnvironment environment)
        {
            this.environment = environment;
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

            var source = $"/img/{guid}.{extension.Name}";
            var filePath = $"{this.environment.ContentRootPath}/wwwroot{source}";

            using var stream = new FileStream(filePath, FileMode.Create);
            await formFile.CopyToAsync(stream);

            var imageInputModel = new ImageInputModel
            {
                Source = source,
            };

            return imageInputModel;
        }
    }
}
