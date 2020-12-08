using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Boulders.Boulders
{
    public class BouldersViewModel
    {
        public CommonViewModel Common { get; set; }

        public IEnumerable<BoulderViewModel> Boulders { get; set; }
    }
}
