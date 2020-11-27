using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Cities
{
    public class CitiesViewModel : PaginationBaseViewModel
    {
        public IEnumerable<CityViewModel> Cities { get; set; }
    }
}
