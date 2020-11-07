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
    public class CountriesService : ICountriesService
    {
        private readonly IDeletableEntityRepository<Country> countriesRepository;

        public CountriesService(IDeletableEntityRepository<Country> countriesRepository)
        {
            this.countriesRepository = countriesRepository;
        }

        public int CountCountries(Expression<Func<Country, bool>> predicate = null)
        {
            var countries = this.countriesRepository
                .AllAsNoTracking();

            if (predicate != null)
            {
                countries = countries.Where(predicate);
            }

            return countries.Count();
        }

        public T GetCountry<T>(Expression<Func<Country, bool>> predicate)
        {
            var country = this.countriesRepository
                .AllAsNoTracking()
                .Where(predicate)
                .To<T>()
                .FirstOrDefault();

            return country;
        }

        public IEnumerable<T> GetCountries<T>(Expression<Func<Country, bool>> predicate = null, Expression<Func<Country, object>> orderBySelector = null, int? skip = null, int? take = null)
        {
            var countries = this.countriesRepository
                .AllAsNoTracking();

            if (predicate != null)
            {
                countries = countries.Where(predicate);
            }

            if (orderBySelector != null)
            {
                countries = countries.OrderBy(orderBySelector);
            }

            if (skip != null)
            {
                countries = countries.Skip((int)skip);
            }

            if (take != null)
            {
                countries = countries.Take((int)take);
            }

            var mapped = countries
                .To<T>()
                .ToList();

            return mapped;
        }

        public async Task<string> CreateCountry(object inputModel)
        {
            var mapper = AutoMapperConfig.MapperInstance;
            var country = mapper.Map<Country>(inputModel);

            await this.countriesRepository.AddAsync(country);
            await this.countriesRepository.SaveChangesAsync();

            return country.Id;
        }

        public async Task<bool> DeleteCountry(Expression<Func<Country, bool>> predicate)
        {
            var country = this.countriesRepository
                .All()
                .Where(predicate)
                .FirstOrDefault();

            if (country == null)
            {
                return false;
            }

            this.countriesRepository.Delete(country);
            await this.countriesRepository.SaveChangesAsync();

            return true;
        }
    }
}
