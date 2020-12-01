using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.Boulders.Ranking
{
    public class RankingsViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public int StartRank { get; set; }

        public IEnumerable<RankingViewModel> Ranking { get; set; }
    }
}
