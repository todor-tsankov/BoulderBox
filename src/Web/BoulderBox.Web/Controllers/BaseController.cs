using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using BoulderBox.Web.ViewModels.Common;
using BoulderBox.Web.ViewModels.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Controllers
{
    public class BaseController : Controller
    {
        protected async Task<ImageInputModel> SaveImageFileAsync(IFormFile formFile)
        {
            if (!this.IsImage(formFile))
            {
                return null;
            }

            var guid = Guid.NewGuid().ToString();
            var extension = Regex.Match(formFile.FileName, @"\.[a-z]+$");

            var filePath = $"./wwwroot/img/{guid}{extension}";
            var source = $"~/img/{guid}{extension}";

            using var stream = new FileStream(filePath, FileMode.Create);
            await formFile.CopyToAsync(stream);

            var imageInputModel = new ImageInputModel
            {
                Id = guid,
                Source = source,
            };

            return imageInputModel;
        }

        private bool IsImage(IFormFile formFile)
        {
            try
            {
                var img = Image.FromStream(formFile.OpenReadStream());
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
