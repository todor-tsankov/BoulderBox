using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Places.Countries
{
    public class CountriesViewModel
    {
        public CommonViewModel Common { get; set; }

        public IEnumerable<CountryViewModel> Countries { get; set; }
    }
}
