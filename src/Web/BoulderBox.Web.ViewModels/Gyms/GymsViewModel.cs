using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Gyms
{
    public class GymsViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<GymViewModel> Gyms { get; set; }
    }
}
