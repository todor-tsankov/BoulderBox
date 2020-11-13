using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Cities;
using BoulderBox.Web.ViewModels.Images;

namespace BoulderBox.Services.Data.Places
{
    public interface ICitiesService : IBaseService<City>
    {
        Task AddCityAsync(CityInputModel cityInput, ImageInputModel imageInput);
    }
}
