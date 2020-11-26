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

        public IActionResult Index()
        {
            var users = this.applicationUsersService
                .GetMany<RankingViewModel>(orderBySelector: x => x.Points.AllTime, asc: false);

            return this.View(users);
        }
    }
}
