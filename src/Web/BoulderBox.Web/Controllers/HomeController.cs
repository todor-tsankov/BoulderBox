using System.Diagnostics;

using BoulderBox.Services.Data.Boulders;
using BoulderBox.Web.ViewModels;
using BoulderBox.Web.ViewModels.Boulders.Grades;
using BoulderBox.Web.ViewModels.Boulders.Styles;
using BoulderBox.Web.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IGradesService gradesService;
        private readonly IStylesService stylesService;

        public HomeController(IGradesService gradesService, IStylesService stylesService)
        {
            this.gradesService = gradesService;
            this.stylesService = stylesService;
        }

        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel()
            {
                Grades = this.gradesService
                    .GetMany<GradeViewModel>(orderBySelector: x => x.Text, asc: false),
                Styles = this.stylesService
                    .GetMany<StyleViewModel>(),
            };

            return this.View(homeViewModel);
        }

        [Authorize]
        public IActionResult Chat()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult About()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
