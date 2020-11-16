﻿using System.Threading.Tasks;

using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.Cities;
using BoulderBox.Web.ViewModels.Images;

namespace BoulderBox.Services.Data.Places
{
    public class CitiesService : BaseService<City>, ICitiesService
    {
        private readonly IDeletableEntityRepository<City> citiesRepository;

        public CitiesService(IDeletableEntityRepository<City> citiesRepository)
            : base(citiesRepository)
        {
            this.citiesRepository = citiesRepository;
        }

        public async Task AddCityAsync(CityInputModel cityInput, ImageInputModel imageInput)
        {
            var mapper = AutoMapperConfig.MapperInstance;

            var city = mapper.Map<City>(cityInput);
            var image = mapper.Map<Image>(imageInput);

            city.Image = image;

            await this.citiesRepository.AddAsync(city);
            await this.citiesRepository.SaveChangesAsync();
        }
    }
}