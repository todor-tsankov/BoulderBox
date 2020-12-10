using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Places.Cities;

namespace BoulderBox.Services.Data.Places
{
    public interface ICitiesService : IBaseService<City>
    {
        Task AddAsync(CityInputModel cityInput, ImageInputModel imageInput);

        Task EditAsync(string id, CityInputModel cityInput, ImageInputModel imageInput);
    }
}
