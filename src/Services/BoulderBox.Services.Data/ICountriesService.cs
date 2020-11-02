using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Models;

namespace BoulderBox.Services.Data
{
    public interface ICountriesService
    {
        int CountCountries(Expression<Func<Country, bool>>[] predicate);

        IEnumerable<T> GetCountry<T>(Expression<Func<Country, bool>> predicate);

        IEnumerable<T> GetCountries<T>(
           Expression<Func<Country, bool>> predicate = null,
           Expression<Func<Country, object>> orderBySelector = null,
           int? skip = null,
           int? take = null);

        Task<string> CreateCountry<T>(T inputModel);

        Task<bool> DeleteCountry(Expression<Func<Country, bool>> predicate);
    }
}
