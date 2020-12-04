using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Forum.Posts;

namespace BoulderBox.Services.Data.Forum
{
    public interface IPostsService : IBaseService<Post>
    {
        Task AddAsync(PostInputModel postInput, ImageInputModel image, string userId);
    }
}
