using System.Collections.Generic;

namespace BoulderBox.Web.ViewModels.ViewComponents
{
    public class SideBarViewModel
    {
        public IEnumerable<SideBarAscentViewModel> LatestAscents { get; set; }

        public IEnumerable<SideBarBoulderViewModel> LatestBoulders { get; set; }

        public SideBarStatistics Statistics { get; set; }
    }
}
