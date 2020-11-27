using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using BoulderBox.Services.Data.Boulders;
using BoulderBox.Services.Data.Places;
using BoulderBox.Web.ViewModels.Boulders;
using BoulderBox.Web.ViewModels.Countries;
using BoulderBox.Web.ViewModels.Grades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.Controllers
{
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
                CurrentPage = pageId,
                ItemsCount = this.bouldersService.Count(),
                ItemsPerPage = itemsPerPage,
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

            boulder.CountriesSelectItems = this.countriesService
                .GetMany<CountryViewModel>(x => x.Cities.Any(), x => x.Name)
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            boulder.GradesSelectItems = this.gradesService
                .GetMany<GradeViewModel>(orderBySelector: x => x.Text)
                .Select(x => new SelectListItem(x.Text, x.Id))
                .ToList();

            return this.View(boulder);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BoulderInputModel boulderInput, IFormFile formFile)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
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
    }
}
