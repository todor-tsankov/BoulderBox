using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using BoulderBox.Services.Data.Boulders;
using BoulderBox.Services.Data.Places;
using BoulderBox.Web.Controllers;
using BoulderBox.Web.ViewModels.Boulders.Boulders;
using BoulderBox.Web.ViewModels.Boulders.Grades;
using BoulderBox.Web.ViewModels.Places.Countries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.Areas.Boulders.Controllers
{
    [Area("Boulders")]
    public class BouldersController : BaseController
    {
        private readonly IBouldersService bouldersService;
        private readonly ICountriesService countriesService;
        private readonly IGradesService gradesService;

        public BouldersController(
            IBouldersService bouldersService,
            ICountriesService countriesService,
            IGradesService gradesService)
        {
            this.bouldersService = bouldersService;
            this.countriesService = countriesService;
            this.gradesService = gradesService;
        }

        public IActionResult Index(int pageId = 1)
        {
            var itemsPerPage = 8;
            var skip = itemsPerPage * (pageId - 1);

            var bouldersViewModel = new BouldersViewModel()
            {
                Boulders = this.bouldersService
                    .GetMany<BoulderViewModel>(
                        orderBySelector: x => x.Name,
                        skip: skip,
                        take: itemsPerPage),

                Pagination = this.GetPaginationModel(pageId, this.bouldersService.Count(), itemsPerPage),
            };

            return this.View(bouldersViewModel);
        }

        public IActionResult Details(string id)
        {
            var boulder = this.bouldersService
                .GetSingle<BoulderDetailsViewModel>(x => x.Id == id);

            return this.View(boulder);
        }

        public IActionResult Create()
        {
            var boulder = new BoulderInputModel();
            this.SetListItems(boulder);

            return this.View(boulder);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BoulderInputModel boulderInput, IFormFile formFile)
        {
            if (!this.ModelState.IsValid)
            {
                this.SetListItems(boulderInput);
                return this.View(boulderInput);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var image = await this.SaveImageFileAsync(formFile);
            await this.bouldersService.AddBoulderAsync(boulderInput, userId, image);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.bouldersService.DeleteAsync(x => x.Id == id);

            return this.RedirectToAction("Index");
        }

        private void SetListItems(BoulderInputModel boulder)
        {
            boulder.CountriesSelectItems = this.countriesService
                            .GetMany<CountryViewModel>(x => x.Cities.Any(), x => x.Name)
                            .Select(x => new SelectListItem(x.Name, x.Id))
                            .ToList();

            boulder.GradesSelectItems = this.gradesService
                .GetMany<GradeViewModel>(orderBySelector: x => x.Text)
                .Select(x => new SelectListItem(x.Text, x.Id))
                .ToList();
        }
    }
}
