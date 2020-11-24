using System.Threading.Tasks;

using BoulderBox.Services.Data.Boulders;
using BoulderBox.Services.Data.Users;
using BoulderBox.Web.ViewModels.ViewComponents;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.ViewComponents
{
    public class SideBarViewComponent : ViewComponent
    {
        private readonly IAscentsService ascentsService;
        private readonly IBouldersService bouldersService;
        private readonly IApplicationUsersService usersService;

        public SideBarViewComponent(
            IAscentsService ascentsService,
            IBouldersService bouldersService,
            IApplicationUsersService usersService)
        {
            this.ascentsService = ascentsService;
            this.bouldersService = bouldersService;
            this.usersService = usersService;
        }

        public IViewComponentResult Invoke()
        {
            var ascentsBoulders = new SideBarViewModel()
            {
                LatestAscents = this.ascentsService
                    .GetMany<SideBarAscentViewModel>(orderBySelector: x => x.Date, asc: false, take: 8),
                LatestBoulders = this.bouldersService
                    .GetMany<SideBarBoulderViewModel>(orderBySelector: x => x.CreatedOn, asc: false, take: 8),
                Statistics = new SideBarStatistics()
                {
                    MembersCount = this.usersService.Count(),
                    BouldersCount = this.bouldersService.Count(),
                    AscentsCount = this.ascentsService.Count(),
                }
            };

            return this.View(ascentsBoulders);
        }
    }
}
