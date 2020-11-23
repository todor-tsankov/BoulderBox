using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.ForumPosts;
using BoulderBox.Web.ViewModels.Images;

namespace BoulderBox.Services.Data.Forum
{
    public interface IForumPostsService : IBaseService<ForumPost>
    {
        Task Create(ForumPostInputModel forumPostInput, ImageInputModel image, string userId);
    }
}
