using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Models;

namespace BoulderBox.Services.Data
{
    public interface IImagesService
    {
        int CountImages(Expression<Func<Image, bool>>[] predicate);

        IEnumerable<T> GetImage<T>(Expression<Func<Image, bool>> predicate);

        IEnumerable<T> GetImages<T>(
           Expression<Func<Image, bool>> predicate = null,
           Expression<Func<Image, object>> orderBySelector = null,
           int? skip = null,
           int? take = null);

        Task<string> CreateImage<T>(T inputModel);

        Task<bool> DeleteImage(Expression<Func<Image, bool>> predicate);
    }
}
