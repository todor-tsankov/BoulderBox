using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using BoulderBox.Web.ViewModels.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Controllers
{
    public class BaseController : Controller
    {
        protected async Task<ImageInputModel> SaveImageFileAsync(IFormFile formFile)
        {
            if (formFile == null || formFile.Length < 1)
            {
                return null;
            }

            var guid = Guid.NewGuid().ToString();
            var filePath = $"./wwwroot/img/{guid}.jpg";
            var source = $"img/{guid}.jpg";

            using var stream = new FileStream(filePath, FileMode.Create);
            await formFile.CopyToAsync(stream);

            var imageInputModel = new ImageInputModel
            {
                Id = guid,
                Source = source,
            };

            return imageInputModel;
        }
    }
}
