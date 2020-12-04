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
        Task<bool> AddAsync(BoulderInputModel boulderInput, string authorId, ImageInputModel image);
    }
}
