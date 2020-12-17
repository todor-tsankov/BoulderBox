using System.Linq;
using System.Threading.Tasks;

using BoulderBox.Common;
using BoulderBox.Services.Data.Boulders;
using BoulderBox.Web.Areas.Administration.Controllers.Common;
using BoulderBox.Web.ViewModels.Boulders.Ascents;
using BoulderBox.Web.ViewModels.Boulders.Grades;
using BoulderBox.Web.ViewModels.Boulders.Styles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.Areas.Administration.Controllers
{
    public class AscentsController : AdministrationController
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

        public IActionResult Edit(string id)
        {
            var existsAscent = this.ascentsService
                .Exists(x => x.Id == id);

            if (!existsAscent)
            {
                return this.NotFound();
            }

            var ascent = new AscentEditViewModel()
            {
                Id = id,
                AscentInput = this.ascentsService
                    .GetSingle<AscentInputModel>(x => x.Id == id),
            };

            this.SetListItems(ascent.AscentInput);

            return this.View(ascent);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, AscentInputModel ascentInput)
        {
            var existsAscent = this.ascentsService
                .Exists(x => x.Id == id);

            if (!existsAscent)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                var ascent = new AscentEditViewModel()
                {
                    Id = id,
                    AscentInput = ascentInput,
                };

                this.SetListItems(ascent.AscentInput);

                return this.View(ascent);
            }

            await this.ascentsService.EditAsync(id, ascentInput);
            this.TempData[GlobalConstants.MessageKey] = $"Successfully edited ascent!";

            return this.RedirectToAction("Index", "Ascents", new { area = "Boulders" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var existsAscent = this.ascentsService
                .Exists(x => x.Id == id);

            if (!existsAscent)
            {
                return this.NotFound();
            }

            await this.ascentsService.DeleteAsync(x => x.Id == id);
            this.TempData[GlobalConstants.MessageKey] = $"Successfully deleted ascent!";

            return this.RedirectToAction("Index", "Ascents", new { area = "Boulders" });
        }

        private void SetListItems(AscentInputModel ascent)
        {
            ascent.GradesSelectListItems = this.gradesService
                                .GetMany<GradeViewModel>(orderBySelector: x => x.Text)
                                .Select(x => new SelectListItem()
                                {
                                    Value = x.Id,
                                    Text = x.Text,
                                })
                                .ToList();

            ascent.StylesSelectListItems = this.stylesService
                .GetMany<StyleViewModel>()
                .Select(x => new SelectListItem()
                {
                    Value = x.Id,
                    Text = x.LongText,
                })
                .ToList();
        }
    }
}
