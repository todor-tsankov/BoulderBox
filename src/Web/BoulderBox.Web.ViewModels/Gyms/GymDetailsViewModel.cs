using System.Collections.Generic;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Gyms
{
    public class GymDetailsViewModel : IMapFrom<Gym>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CityId { get; set; }

        public string CityName { get; set; }

        public string ImageSource { get; set; }

        public string Description { get; set; }

        public ICollection<GymDetailsBoulderViewModel> Boulders { get; set; }
    }
}
