﻿using System.Linq;
using System.Threading.Tasks;

using BoulderBox.Services.Data.Files;
using BoulderBox.Services.Data.Places;
using BoulderBox.Web.ViewModels.Cities;
using BoulderBox.Web.ViewModels.Countries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.Controllers
{
    public class CitiesController : BaseController
    {
        private readonly ICitiesService citiesService;
        private readonly ICountriesService countriesService;

        public CitiesController(ICitiesService citiesService, ICountriesService countriesService)
        {
            this.citiesService = citiesService;
            this.countriesService = countriesService;
        }

        public IActionResult Index(int pageId = 1)
        {
            var skip = DefaultItemsPerPage * (pageId - 1);

            var citiesViewModel = new CitiesViewModel()
            {
                Cities = this.citiesService
                    .GetMany<CityViewModel>(
                        orderBySelector: x => x.Name,
                        skip: skip,
                        take: DefaultItemsPerPage),

                Pagination = this.GetPaginationModel(pageId, this.citiesService.Count()),
            };

            return this.View(citiesViewModel);
        }

        public IActionResult Details(string id)
        {
            var city = this.citiesService
                .GetSingle<CityDetailsViewModel>(x => x.Id == id);

            return this.View(city);
        }

        public IActionResult Create()
        {
            var city = new CityInputModel
            {
                CountriesSelectListItems = this.countriesService
                    .GetMany<CountryViewModel>(orderBySelector: x => x.Name)
                    .Select(x => new SelectListItem(x.Name, x.Id))
                    .ToList(),
            };

            return this.View(city);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CityInputModel cityInput, IFormFile formFile)
        {
            var existCountry = this.countriesService.Exists(x => x.Id == cityInput.CountryId);

            if (!this.ModelState.IsValid || !existCountry)
            {
                var city = new CityInputModel
                {
                    CountriesSelectListItems = this.countriesService
                        .GetMany<CountryViewModel>(orderBySelector: x => x.Name)
                        .Select(x => new SelectListItem(x.Name, x.Id))
                        .ToList(),
                };

                return this.View(city);
            }

            var image = await this.SaveImageFileAsync(formFile);
            await this.citiesService.AddCityAsync(cityInput, image);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.citiesService.DeleteAsync(x => x.Id == id);

            return this.RedirectToAction("Index");
        }
    }
}
