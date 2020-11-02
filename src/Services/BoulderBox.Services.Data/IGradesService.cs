using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Models;

namespace BoulderBox.Services.Data
{
    public interface IGradesService
    {
        int CountGrades(Expression<Func<Grade, bool>>[] predicate);

        IEnumerable<T> GetGrade<T>(Expression<Func<Grade, bool>> predicate);

        IEnumerable<T> GetGrades<T>(
           Expression<Func<Grade, bool>> predicate = null,
           Expression<Func<Grade, object>> orderBySelector = null,
           int? skip = null,
           int? take = null);

        Task<string> CreateGrade<T>(T inputModel);

        Task<bool> DeleteGrade(Expression<Func<Grade, bool>> predicate);
    }
}
