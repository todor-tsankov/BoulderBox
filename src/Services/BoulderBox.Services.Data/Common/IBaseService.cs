using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Common.Models;

namespace BoulderBox.Services.Data.Common
{
    public interface IBaseService<TModel>
        where TModel : class, IDeletableEntity
    {
        int Count(Expression<Func<TModel, bool>> predicate);

        T GetSingle<T>(Expression<Func<TModel, bool>> predicate);

        IEnumerable<T> GetMany<T>(
           Expression<Func<TModel, bool>> predicate = null,
           Expression<Func<TModel, object>> orderBySelector = null,
           int? skip = null,
           int? take = null);

        Task<bool> Delete(Expression<Func<TModel, bool>> predicate);
    }
}
