using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Boulders.Boulders
{
    public class BouldersViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<BoulderViewModel> Boulders { get; set; }
    }
}
