using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Boulders.Ascents;

namespace BoulderBox.Services.Data.Boulders
{
    public interface IAscentsService : IBaseService<Ascent>
    {
        Task AddAsync(AscentInputModel ascentInput, string userId);

        Task EditAsync(string id, AscentInputModel ascentInput);
    }
}
