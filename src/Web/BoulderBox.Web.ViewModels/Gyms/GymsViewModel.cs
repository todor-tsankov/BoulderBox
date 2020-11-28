using BoulderBox.Web.ViewModels.Common;

using System.Collections.Generic;

namespace BoulderBox.Web.ViewModels.Gyms
{
    public class GymsViewModel : PaginationViewModel
    {
        public IEnumerable<GymViewModel> Gyms { get; set; }
    }
}
