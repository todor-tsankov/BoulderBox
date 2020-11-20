using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Forum;
using BoulderBox.Web.ViewModels.Images;

namespace BoulderBox.Services.Data.Forum
{
    public interface IForumCategoriesService : IBaseService<ForumCategory>
    {
        Task Create(ForumCategoryInputModel forumCategoryInput, ImageInputModel imageInput);
    }
}
