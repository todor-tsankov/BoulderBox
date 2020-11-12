using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.Countries;
using BoulderBox.Web.ViewModels.Images;

namespace BoulderBox.Services.Data.Places
{
    public class CountriesService : BaseService<Country>, ICountriesService
    {
        private readonly IDeletableEntityRepository<Country> countriesRepository;

        public CountriesService(IDeletableEntityRepository<Country> countriesRepository)
            : base(countriesRepository)
        {
            this.countriesRepository = countriesRepository;
        }

        public async Task<bool> AddCountryAsync(CountryInputModel countryInput, ImageInputModel imageInput = null)
        {
            var mapper = AutoMapperConfig.MapperInstance;
            var country = mapper.Map<Country>(countryInput);

            country.Image = mapper.Map<Image>(imageInput);

            await this.countriesRepository.AddAsync(country);
            await this.countriesRepository.SaveChangesAsync();

            return true;
        }
    }
}
