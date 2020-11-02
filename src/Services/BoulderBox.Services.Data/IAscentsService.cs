using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Models;

namespace BoulderBox.Services.Data
{
    public interface IAscentsService
    {
        int CountAscents(Expression<Func<Ascent, bool>>[] predicate);

        IEnumerable<T> GetAscent<T>(Expression<Func<Ascent, bool>> predicate);

        IEnumerable<T> GetAscents<T>(
           Expression<Func<Ascent, bool>> predicate = null,
           Expression<Func<Ascent, object>> orderBySelector = null,
           int? skip = null,
           int? take = null);

        Task<string> CreateAscent<T>(T inputModel);

        Task<bool> DeleteAscent(Expression<Func<Ascent, bool>> predicate);
    }
}
