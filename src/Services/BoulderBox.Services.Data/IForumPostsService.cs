using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Models;

namespace BoulderBox.Services.Data
{
    public interface IForumPostsService
    {
        int CountForumPosts(Expression<Func<ForumPost, bool>>[] predicate);

        IEnumerable<T> GetForumPost<T>(Expression<Func<ForumPost, bool>> predicate);

        IEnumerable<T> GetForumPosts<T>(
           Expression<Func<ForumPost, bool>> predicate = null,
           Expression<Func<ForumPost, object>> orderBySelector = null,
           int? skip = null,
           int? take = null);

        Task<string> CreateForumPost<T>(T inputModel);

        Task<bool> DeleteForumPost(Expression<Func<Gym, bool>> predicate);
    }
}
