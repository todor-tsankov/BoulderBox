using System.Linq;
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

        public async Task AddAsync(CityInputModel cityInput, ImageInputModel imageInput)
        {
            this.NullCheck(cityInput, nameof(cityInput));

            var city = this.mapper.Map<City>(cityInput);
            city.Image = this.mapper.Map<Image>(imageInput);

            await this.citiesRepository.AddAsync(city);
            await this.citiesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string id, CityInputModel cityInput, ImageInputModel imageInput)
        {
            this.NullCheck(id, nameof(id));
            this.NullCheck(cityInput, nameof(cityInput));

            var city = this.citiesRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (imageInput != null)
            {
                city.Image = this.mapper.Map<Image>(imageInput);
            }

            city.Name = cityInput.Name;
            city.CountryId = cityInput.CountryId;
            city.Description = cityInput.Description;

            await this.citiesRepository.SaveChangesAsync();
        }
    }
}
