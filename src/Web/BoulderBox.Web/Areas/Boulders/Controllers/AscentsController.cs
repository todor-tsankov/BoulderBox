using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using BoulderBox.Services.Data.Boulders;
using BoulderBox.Web.Controllers;
using BoulderBox.Web.ViewModels.Boulders.Ascents;
using BoulderBox.Web.ViewModels.Boulders.Grades;
using BoulderBox.Web.ViewModels.Boulders.Styles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.Areas.Boulders.Controllers
{
    [Area("Boulders")]
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

        public IActionResult Index(int pageId = 1)
        {
            var skip = DefaultItemsPerPage * (pageId - 1);

            var ascentsViewModel = new AscentsViewModel()
            {
                Ascents = this.ascentsService
                    .GetMany<AscentViewModel>(
                        orderBySelector: x => x.Date,
                        asc: false,
                        skip: skip,
                        take: DefaultItemsPerPage),

                Pagination = this.GetPaginationModel(pageId, this.ascentsService.Count()),
            };

            return this.View(ascentsViewModel);
        }

        public IActionResult Create(string id)
        {
            var ascent = new AscentInputModel()
            {
                BoulderId = id,
            };

            this.SetListItems(ascent);
            return this.View(ascent);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AscentInputModel ascentInput)
        {
            if (!this.ModelState.IsValid)
            {
                this.SetListItems(ascentInput);
                return this.View(ascentInput);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.ascentsService.AddAsync(ascentInput, userId);

            return this.RedirectToAction("Index");
        }

        private void SetListItems(AscentInputModel ascentInput)
        {
            ascentInput.GradesSelectListItems = this.gradesService
                .GetMany<GradeViewModel>(orderBySelector: x => x.Text)
                .Select(x => new SelectListItem(x.Text, x.Id))
                .ToList();

            ascentInput.StylesSelectListItems = this.stylesService
                .GetMany<StyleViewModel>(orderBySelector: x => x.CreatedOn)
                .Select(x => new SelectListItem($"{x.LongText} ({x.ShortText})", x.Id))
                .ToList();
        }
    }
}
