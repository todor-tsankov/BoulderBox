using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Models;

namespace BoulderBox.Services.Data
{
    public interface IStylesService
    {
        int CountStyles(Expression<Func<Style, bool>>[] predicate);

        IEnumerable<T> GetStyle<T>(Expression<Func<Style, bool>> predicate);

        IEnumerable<T> GetStyles<T>(
           Expression<Func<Style, bool>> predicate = null,
           Expression<Func<Style, object>> orderBySelector = null,
           int? skip = null,
           int? take = null);

        Task<string> CreateStyle<T>(T inputModel);

        Task<bool> DeleteStyle(Expression<Func<Style, bool>> predicate);
    }
}
