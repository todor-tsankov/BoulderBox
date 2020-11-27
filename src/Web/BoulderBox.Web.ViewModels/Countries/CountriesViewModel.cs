using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Countries
{
    public class CountriesViewModel : PaginationBaseViewModel
    {
        public IEnumerable<CountryViewModel> Countries { get; set; }
    }
}
