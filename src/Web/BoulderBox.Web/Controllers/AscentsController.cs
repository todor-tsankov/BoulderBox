using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using BoulderBox.Services.Data.Boulders;
using BoulderBox.Web.ViewModels.Ascents;
using BoulderBox.Web.ViewModels.Grades;
using BoulderBox.Web.ViewModels.Styles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.Controllers
{
    public class AscentsController : BaseController
    {
        private readonly IAscentsService ascentsService;
        private readonly IGradesService gradesService;
        private readonly IStylesService stylesService;

        public AscentsController(
            IAscentsService ascentsService,
            IGradesService gradesService,
            IStylesService stylesService)
        {
            this.ascentsService = ascentsService;
            this.gradesService = gradesService;
            this.stylesService = stylesService;
        }

        public IActionResult Index()
        {
            var ascents = this.ascentsService
                .GetMany<AscentViewModel>();

            return this.View(ascents);
        }

        public IActionResult Create(string id)
        {
            var ascent = new AscentInputModel()
            {
                BoulderId = id,
            };

            ascent.GradesSelectListItems = this.gradesService
                .GetMany<GradeViewModel>(orderBySelector: x => x.Text)
                .Select(x => new SelectListItem(x.Text, x.Id))
                .ToList();

            ascent.StylesSelectListItems = this.stylesService
                .GetMany<StyleViewModel>(orderBySelector: x => x.CreatedOn)
                .Select(x => new SelectListItem($"{x.LongText} ({x.ShortText})", x.Id))
                .ToList();

            return this.View(ascent);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AscentInputModel ascentInput)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(ascentInput);
            }

            ascentInput.ApplicationUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.ascentsService.Create(ascentInput);

            return this.RedirectToAction("Index");
        }
    }
}
