using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Places.Gyms
{
    public class GymViewModel : IMapFrom<Gym>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CityId { get; set; }

        public virtual string CityName { get; set; }

        public string ImageSource { get; set; }
    }
}
