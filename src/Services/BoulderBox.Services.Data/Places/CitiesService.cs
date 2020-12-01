using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Places.Cities;

namespace BoulderBox.Services.Data.Places
{
    public class CitiesService : BaseService<City>, ICitiesService
    {
        private readonly IDeletableEntityRepository<City> citiesRepository;
        private readonly IMapper mapper;

        public CitiesService(IDeletableEntityRepository<City> citiesRepository, IMapper mapper)
            : base(citiesRepository, mapper)
        {
            this.citiesRepository = citiesRepository;
            this.mapper = mapper;
        }

        public async Task AddCityAsync(CityInputModel cityInput, ImageInputModel imageInput)
        {
            this.NullCheck(cityInput, nameof(cityInput));

            var city = this.mapper.Map<City>(cityInput);
            city.Image = this.mapper.Map<Image>(imageInput);

            await this.citiesRepository.AddAsync(city);
            await this.citiesRepository.SaveChangesAsync();
        }
    }
}
