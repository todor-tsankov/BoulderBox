using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Places.Cities
{
    public class CitiesViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<CityViewModel> Cities { get; set; }
    }
}
