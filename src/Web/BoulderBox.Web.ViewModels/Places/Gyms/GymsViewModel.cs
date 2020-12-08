using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Places.Gyms
{
    public class GymsViewModel
    {
        public CommonViewModel Common { get; set; }

        public IEnumerable<GymViewModel> Gyms { get; set; }
    }
}
