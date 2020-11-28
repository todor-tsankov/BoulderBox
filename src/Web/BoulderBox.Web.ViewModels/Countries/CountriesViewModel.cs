using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Countries
{
    public class CountriesViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<CountryViewModel> Countries { get; set; }
    }
}
