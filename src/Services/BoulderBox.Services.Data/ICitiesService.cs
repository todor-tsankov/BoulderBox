using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Models;

namespace BoulderBox.Services.Data
{
    public interface ICitiesService
    {
        int CountCities(Expression<Func<City, bool>> predicate = null);

        T GetCity<T>(Expression<Func<City, bool>> predicate);

        IEnumerable<T> GetCities<T>(
           Expression<Func<City, bool>> predicate = null,
           Expression<Func<City, object>> orderBySelector = null,
           int? skip = null,
           int? take = null);

        Task<string> CreateCity(object inputModel);

        Task<bool> DeleteCity(Expression<Func<City, bool>> predicate);
    }
}
