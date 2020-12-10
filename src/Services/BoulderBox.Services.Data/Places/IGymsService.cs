using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Places.Gyms;

namespace BoulderBox.Services.Data.Places
{
    public interface IGymsService : IBaseService<Gym>
    {
        Task AddAsync(GymInputModel gymInput, ImageInputModel image);

        Task EditAsync(string id, GymInputModel gymInput, ImageInputModel imageInput);
    }
}
