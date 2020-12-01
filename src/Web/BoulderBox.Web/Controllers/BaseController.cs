using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using BoulderBox.Web.ViewModels.Common;
using BoulderBox.Web.ViewModels.Files.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Controllers
{
    public class BaseController : Controller
    {
        protected const int DefaultFirstPage = 1;
        protected const int DefaultItemsPerPage = 12;

        protected PaginationViewModel GetPaginationModel(int currentPage, int count, int itemsPerPage = DefaultItemsPerPage)
        {
            var lastPage = count / itemsPerPage;

            if (count % itemsPerPage != 0)
            {
                lastPage++;
            }

            if (lastPage < DefaultFirstPage)
            {
                lastPage = DefaultFirstPage;
            }

            return new PaginationViewModel(DefaultFirstPage, lastPage, currentPage);
        }

        protected async Task<ImageInputModel> SaveImageFileAsync(IFormFile formFile)
        {
            if (!IsImage(formFile))
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

        private static bool IsImage(IFormFile formFile)
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
