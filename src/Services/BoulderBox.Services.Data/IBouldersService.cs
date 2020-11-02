using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Models;

namespace BoulderBox.Services.Data
{
    public interface IBouldersService
    {
        int CountBoulders(Expression<Func<Boulder, bool>>[] predicate);

        IEnumerable<T> GetBoulder<T>(Expression<Func<Boulder, bool>> predicate);

        IEnumerable<T> GetBoulders<T>(
           Expression<Func<Boulder, bool>> predicate = null,
           Expression<Func<Boulder, object>> orderBySelector = null,
           int? skip = null,
           int? take = null);

        Task<string> CreateBoulder<T>(T inputModel);

        Task<bool> DeleteBoulder(Expression<Func<Boulder, bool>> predicate);
    }
}
