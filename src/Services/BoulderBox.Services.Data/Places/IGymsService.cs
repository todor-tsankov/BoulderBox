using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Gyms;
using BoulderBox.Web.ViewModels.Images;

namespace BoulderBox.Services.Data.Places
{
    public interface IGymsService : IBaseService<Gym>
    {
        Task<bool> AddGymAsync(GymInputModel gymInput, ImageInputModel image);
    }
}
