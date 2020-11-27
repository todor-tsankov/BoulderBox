using BoulderBox.Services.Data.Users;
using BoulderBox.Web.ViewModels.Ranking;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Controllers
{
    public class RankingController : BaseController
    {
        private readonly IApplicationUsersService applicationUsersService;

        public RankingController(IApplicationUsersService applicationUsersService)
        {
            this.applicationUsersService = applicationUsersService;
        }

        public IActionResult Index(int pageId = 1)
        {
            var itemsPerPage = 12;
            var skip = itemsPerPage * (pageId - 1);

            var rankingsViewModel = new RankingsViewModel()
            {
                Ranking = this.applicationUsersService
                    .GetMany<RankingViewModel>(
                        orderBySelector: x => x.Points.Yearly,
                        asc: false,
                        skip: skip,
                        take: itemsPerPage),
                CurrentPage = pageId,
                ItemsCount = this.applicationUsersService.Count(),
                ItemsPerPage = itemsPerPage,
                StartRank = skip + 1,
            };

            return this.View(rankingsViewModel);
        }
    }
}
