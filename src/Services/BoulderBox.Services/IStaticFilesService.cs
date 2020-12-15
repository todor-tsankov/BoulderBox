using System.Threading.Tasks;

using BoulderBox.Web.ViewModels.Files.Images;
using Microsoft.AspNetCore.Http;

namespace BoulderBox.Services
{
    public interface IStaticFilesService
    {
        Task<ImageInputModel> SaveImageAsync(IFormFile formFile);
    }
}
