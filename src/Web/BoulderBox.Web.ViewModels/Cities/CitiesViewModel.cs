using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Cities
{
    public class CitiesViewModel : PaginationViewModel
    {
        public IEnumerable<CityViewModel> Cities { get; set; }
    }
}
