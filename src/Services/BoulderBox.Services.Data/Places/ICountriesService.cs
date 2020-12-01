using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Places.Countries;

namespace BoulderBox.Services.Data.Places
{
    public interface ICountriesService : IBaseService<Country>
    {
        Task<bool> AddCountryAsync(CountryInputModel countryInput, ImageInputModel imageInput = null);
    }
}
