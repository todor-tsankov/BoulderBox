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
            var skip = DefaultItemsPerPage * (pageId - 1);

            var rankingsViewModel = new RankingsViewModel()
            {
                Ranking = this.applicationUsersService
                    .GetMany<RankingViewModel>(
                        orderBySelector: x => x.Points.Yearly,
                        asc: false,
                        skip: skip,
                        take: DefaultItemsPerPage),

                Pagination = this.GetPaginationModel(pageId, this.applicationUsersService.Count()),
            };

            return this.View(rankingsViewModel);
        }
    }
}
