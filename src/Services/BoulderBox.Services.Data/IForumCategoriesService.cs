using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Models;

namespace BoulderBox.Services.Data
{
    public interface IForumCategoriesService
    {
        int CountForumCategories(Expression<Func<ForumCategory, bool>>[] predicate);

        IEnumerable<T> GetForumCategory<T>(Expression<Func<ForumCategory, bool>> predicate);

        IEnumerable<T> GetForumCategories<T>(
           Expression<Func<ForumCategory, bool>> predicate = null,
           Expression<Func<ForumCategory, object>> orderBySelector = null,
           int? skip = null,
           int? take = null);

        Task<string> CreateForumCategory<T>(T inputModel);

        Task<bool> DeleteForumCategory(Expression<Func<ForumCategory, bool>> predicate);
    }
}
