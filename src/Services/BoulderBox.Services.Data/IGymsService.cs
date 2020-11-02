using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Models;

namespace BoulderBox.Services.Data
{
    public interface IGymsService
    {
        int CountGyms(Expression<Func<Gym, bool>>[] predicate);

        IEnumerable<T> GetGym<T>(Expression<Func<Gym, bool>> predicate);

        IEnumerable<T> GetGyms<T>(
           Expression<Func<Gym, bool>> predicate = null,
           Expression<Func<Gym, object>> orderBySelector = null,
           int? skip = null,
           int? take = null);

        Task<string> CreateGym<T>(T inputModel);

        Task<bool> DeleteGym(Expression<Func<Gym, bool>> predicate);
    }
}
