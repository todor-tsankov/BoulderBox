using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Services.Data
{
    public class CitiesService : ICitiesService
    {
        private readonly IDeletableEntityRepository<City> citiesRepository;

        public CitiesService(IDeletableEntityRepository<City> citiesRepository)
        {
            this.citiesRepository = citiesRepository;
        }

        public int CountCities(Expression<Func<City, bool>> predicate = null)
        {
            var cities = this.citiesRepository
                .AllAsNoTracking();

            if (predicate != null)
            {
                cities = cities.Where(predicate);
            }

            return cities.Count();
        }

        public T GetCity<T>(Expression<Func<City, bool>> predicate)
        {
            var city = this.citiesRepository
                .AllAsNoTracking()
                .Where(predicate)
                .To<T>()
                .FirstOrDefault();

            return city;
        }

        public IEnumerable<T> GetCities<T>(Expression<Func<City, bool>> predicate = null, Expression<Func<City, object>> orderBySelector = null, int? skip = null, int? take = null)
        {
            var cities = this.citiesRepository
                .AllAsNoTracking();

            if (predicate != null)
            {
                cities = cities.Where(predicate);
            }

            if (orderBySelector != null)
            {
                cities = cities.OrderBy(orderBySelector);
            }

            if (skip != null)
            {
                cities = cities.Skip((int)skip);
            }

            if (take != null)
            {
                cities = cities.Take((int)take);
            }

            var mapped = cities
                .To<T>()
                .ToList();

            return mapped;
        }

        public async Task<string> CreateCity(object inputModel)
        {
            var mapper = AutoMapperConfig.MapperInstance;
            var city = mapper.Map<City>(inputModel);

            await this.citiesRepository.AddAsync(city);
            await this.citiesRepository.SaveChangesAsync();

            return city.Id;
        }

        public async Task<bool> DeleteCity(Expression<Func<City, bool>> predicate)
        {
            var city = this.citiesRepository
                .All()
                .Where(predicate)
                .FirstOrDefault();

            if (city == null)
            {
                return false;
            }

            this.citiesRepository.Delete(city);
            await this.citiesRepository.SaveChangesAsync();

            return true;
        }
    }
}
