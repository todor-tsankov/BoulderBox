using System.Collections.Generic;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.Ascents;

namespace BoulderBox.Web.ViewModels.Users
{
    public class UserDetailsViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int PointsWeekly { get; set; }

        public int PointsMonthly { get; set; }

        public int PointsYearly { get; set; }

        public int PointsAllTime { get; set; }

        public string Bio { get; set; }

        public string ImageSource { get; set; }

        public virtual ICollection<AscentViewModel> Ascents { get; set; }
    }
}
