using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Places.Countries;

namespace BoulderBox.Services.Data.Places
{
    public class CountriesService : BaseService<Country>, ICountriesService
    {
        private readonly IDeletableEntityRepository<Country> countriesRepository;
        private readonly IMapper mapper;

        public CountriesService(IDeletableEntityRepository<Country> countriesRepository, IMapper mapper)
            : base(countriesRepository, mapper)
        {
            this.countriesRepository = countriesRepository;
            this.mapper = mapper;
        }

        public async Task<bool> AddAsync(CountryInputModel countryInput, ImageInputModel imageInput = null)
        {
            this.NullCheck(countryInput, nameof(countryInput));

            var country = this.mapper.Map<Country>(countryInput);
            country.Image = this.mapper.Map<Image>(imageInput);

            await this.countriesRepository.AddAsync(country);
            await this.countriesRepository.SaveChangesAsync();

            return true;
        }

        public async Task EditAsync(string id, CountryInputModel countryInput, ImageInputModel imageInput)
        {
            this.NullCheck(id, nameof(id));
            this.NullCheck(countryInput, nameof(countryInput));

            var country = this.countriesRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (imageInput != null)
            {
                var newImage = this.mapper.Map<Image>(imageInput);

                country.Image = newImage;
            }

            country.Name = countryInput.Name;
            country.CountryCode = countryInput.CountryCode;
            country.Description = countryInput.Description;

            await this.countriesRepository.SaveChangesAsync();
        }
    }
}
