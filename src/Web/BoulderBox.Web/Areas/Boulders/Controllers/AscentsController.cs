using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

using BoulderBox.Common;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Boulders;
using BoulderBox.Web.Controllers;
using BoulderBox.Web.ViewModels.Boulders.Ascents;
using BoulderBox.Web.ViewModels.Boulders.Grades;
using BoulderBox.Web.ViewModels.Boulders.Styles;
using BoulderBox.Web.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.Areas.Boulders.Controllers
{
    [Area("Boulders")]
    public class AscentsController : BaseController
    {
        private readonly IAscentsService ascentsService;
        private readonly IBouldersService bouldersService;
        private readonly IGradesService gradesService;
        private readonly IStylesService stylesService;

        public AscentsController(
            IAscentsService ascentsService,
            IBouldersService bouldersService,
            IGradesService gradesService,
            IStylesService stylesService)
        {
            this.ascentsService = ascentsService;
            this.bouldersService = bouldersService;
            this.gradesService = gradesService;
            this.stylesService = stylesService;
        }

        public IActionResult Index(SortingInputModel sorting, int pageId = 1)
        {
            if (sorting.OrderBy == null)
            {
                sorting = new SortingInputModel()
                {
                    OrderBy = "Date",
                    Ascending = false,
                };
            }

            var orderBySelector = GetOrderBySelector(sorting);
            var skip = DefaultItemsPerPage * (pageId - 1);

            var ascentsViewModel = new AscentsViewModel()
            {
                Ascents = this.ascentsService
                    .GetMany<AscentViewModel>(
                        orderBySelector: orderBySelector,
                        asc: sorting.Ascending,
                        skip: skip,
                        take: DefaultItemsPerPage),

                Common = new CommonViewModel()
                {
                    Pagination = this.GetPaginationModel(pageId, this.ascentsService.Count()),
                    Sorting = sorting,
                },
            };

            return this.View(ascentsViewModel);
        }

        [Authorize]
        public IActionResult Create(string id)
        {
            var existsBoulder = this.bouldersService
                .Exists(x => x.Id == id);

            if (!existsBoulder)
            {
                return this.NotFound();
            }

            var ascent = new AscentInputModel()
            {
                BoulderId = id,
            };

            this.SetCreateListItems(ascent);
            return this.View(ascent);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(AscentInputModel ascentInput)
        {
            if (!this.ModelState.IsValid)
            {
                this.SetCreateListItems(ascentInput);
                return this.View(ascentInput);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.ascentsService.AddAsync(ascentInput, userId);

            this.TempData[GlobalConstants.MessageKey] = "Successfully added ascent!";

            return this.RedirectToAction("Index", "Ascents", new { area = "Boulders" });
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var valid = this.ascentsService
                .Exists(x => x.Id == id && x.ApplicationUserId == userId);

            if (!valid)
            {
                return this.Forbid();
            }

            var ascent = new AscentEditViewModel()
            {
                Id = id,
                AscentInput = this.ascentsService
                    .GetSingle<AscentInputModel>(x => x.Id == id),
            };

            this.SetEditListItems(ascent.AscentInput);

            return this.View(ascent);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, AscentInputModel ascentInput)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var valid = this.ascentsService
                .Exists(x => x.Id == id && x.ApplicationUserId == userId);

            if (!valid)
            {
                return this.Forbid();
            }

            if (!this.ModelState.IsValid)
            {
                var ascent = new AscentEditViewModel()
                {
                    Id = id,
                    AscentInput = ascentInput,
                };

                this.SetEditListItems(ascent.AscentInput);

                return this.View(ascent);
            }

            await this.ascentsService.EditAsync(id, ascentInput);
            this.TempData[GlobalConstants.MessageKey] = "Successfully edited ascent!";

            return this.RedirectToAction("Index", "Ascents", new { area = "Boulders" });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var valid = this.ascentsService
                .Exists(x => x.Id == id && x.ApplicationUserId == userId);

            if (!valid)
            {
                return this.Forbid();
            }

            await this.ascentsService.DeleteAsync(x => x.Id == id);
            this.TempData[GlobalConstants.MessageKey] = "Successfully deleted ascent!";

            return this.RedirectToAction("Index", "Ascents", new { area = "Boulders" });
        }

        private static Expression<Func<Ascent, object>> GetOrderBySelector(SortingInputModel sortingModel)
        {
            Expression<Func<Ascent, object>> orderBySelect;

            orderBySelect = sortingModel.OrderBy switch
            {
                "Date" => x => x.Date,
                "Grade" => x => x.Grade.Text,
                "Stars" => x => x.Stars,
                _ => x => x.Date,
            };

            return orderBySelect;
        }

        private void SetCreateListItems(AscentInputModel ascentInput)
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

        private void SetEditListItems(AscentInputModel ascent)
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
