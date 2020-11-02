using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Models;

namespace BoulderBox.Services.Data
{
    public interface IForumCommentsService
    {
        int CountForumComments(Expression<Func<ForumComment, bool>>[] predicate);

        IEnumerable<T> GetForumComment<T>(Expression<Func<ForumComment, bool>> predicate);

        IEnumerable<T> GetForumComments<T>(
           Expression<Func<ForumComment, bool>> predicate = null,
           Expression<Func<ForumComment, object>> orderBySelector = null,
           int? skip = null,
           int? take = null);

        Task<string> CreateForumComment<T>(T inputModel);

        Task<bool> DeleteForumComment(Expression<Func<ForumComment, bool>> predicate);
    }
}
