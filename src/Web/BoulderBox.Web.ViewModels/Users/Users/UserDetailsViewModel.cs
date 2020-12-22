using System.Collections.Generic;

using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.Boulders.Ascents;

namespace BoulderBox.Web.ViewModels.Users.Users
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

        public int AscentsCount { get; set; }

        public int BouldersCount { get; set; }

        public int PostsCount { get; set; }

        public int CommentsCount { get; set; }

        [IgnoreMap]
        public virtual IEnumerable<AscentGroupViewModel> Ascents { get; set; }
    }
}
