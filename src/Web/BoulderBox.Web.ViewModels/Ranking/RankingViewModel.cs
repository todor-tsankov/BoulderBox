using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Ranking
{
    public class RankingViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int PointsWeekly { get; set; }

        public int PointsMonthly { get; set; }

        public int PointsYearly { get; set; }

        public int PointsAllTime { get; set; }

        public string ImageSource { get; set; }
    }
}
