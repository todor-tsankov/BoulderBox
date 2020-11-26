using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Ascents;

namespace BoulderBox.Services.Data.Boulders
{
    public interface IAscentsService : IBaseService<Ascent>
    {
        Task Create(AscentInputModel ascentInput, string userId);
    }
}
