using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Boulders
{
    public class BouldersViewModel : PaginationBaseViewModel
    {
        public IEnumerable<BoulderViewModel> Boulders { get; set; }
    }
}
