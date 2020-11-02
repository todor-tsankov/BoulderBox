using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Models;

namespace BoulderBox.Services.Data
{
    public interface IApplicationUsersService
    {
        int CountApplicationUsers(Expression<Func<ApplicationUser, bool>>[] predicate);

        IEnumerable<T> GetApplicationUser<T>(Expression<Func<ApplicationUser, bool>> predicate);

        IEnumerable<T> GetApplicationUsers<T>(
           Expression<Func<ApplicationUser, bool>> predicate = null,
           Expression<Func<ApplicationUser, object>> orderBySelector = null,
           int? skip = null,
           int? take = null);

        Task<string> CreateApplicationUser<T>(T inputModel);

        Task<bool> DeleteApplicationUser(Expression<Func<ApplicationUser, bool>> predicate);
    }
}
