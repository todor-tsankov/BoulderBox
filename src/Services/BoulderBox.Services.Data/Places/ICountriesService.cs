using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Countries;
using BoulderBox.Web.ViewModels.Images;

namespace BoulderBox.Services.Data.Places
{
    public interface ICountriesService : IBaseService<Country>
    {
        Task<bool> AddCountryAsync(CountryInputModel countryInput);
    }
}
