using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Boulders.Ascents
{
    public class AscentsViewModel
    {
        public CommonViewModel Common { get; set; }

        public IEnumerable<AscentViewModel> Ascents { get; set; }
    }
}
