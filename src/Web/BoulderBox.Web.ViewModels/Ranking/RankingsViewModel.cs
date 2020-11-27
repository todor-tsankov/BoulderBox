using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Ranking
{
    public class RankingsViewModel : PaginationBaseViewModel
    {
        public int StartRank { get; set; }

        public IEnumerable<RankingViewModel> Ranking { get; set; }
    }
}
