using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Forum.Categories;

namespace BoulderBox.Services.Data.Forum
{
    public interface ICategoriesService : IBaseService<Category>
    {
        Task AddAsync(CategoryInputModel categoryInput, ImageInputModel imageInput);
    }
}
