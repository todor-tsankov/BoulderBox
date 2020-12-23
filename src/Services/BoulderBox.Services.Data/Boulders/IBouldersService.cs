using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Boulders;
using BoulderBox.Web.ViewModels.Boulders.Boulders;
using BoulderBox.Web.ViewModels.Files.Images;

namespace BoulderBox.Services.Data.Boulders
{
    public interface IBouldersService : IBaseService<Boulder>
    {
        Task AddAsync(BoulderInputModel boulderInput, string authorId, ImageInputModel image);

        Task EditAsync(string id, BoulderInputModel boulderInput, ImageInputModel imageInput);
    }
}
