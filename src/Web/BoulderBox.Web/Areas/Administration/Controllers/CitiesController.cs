﻿using System.Linq;
using System.Threading.Tasks;

using BoulderBox.Services.Data.Places;
using BoulderBox.Web.Areas.Administration.Controllers.Common;
using BoulderBox.Web.ViewModels.Places.Cities;
using BoulderBox.Web.ViewModels.Places.Countries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.Areas.Administration.Controllers
{
    public class CitiesController : AdministrationController
    {
        private readonly ICountriesService countriesService;
        private readonly ICitiesService citiesService;

        public CitiesController(ICountriesService countriesService, ICitiesService citiesService)
        {
            this.countriesService = countriesService;
            this.citiesService = citiesService;
        }

        public IActionResult Create()
        {
            var city = new CityInputModel();
            this.SetListItems(city);

            return this.View(city);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CityInputModel cityInput, IFormFile formFile)
        {
            var existCountry = this.countriesService.Exists(x => x.Id == cityInput.CountryId);

            if (!this.ModelState.IsValid || !existCountry)
            {
                this.SetListItems(cityInput);
                return this.View(cityInput);
            }

            var image = await this.SaveImageFileAsync(formFile);
            await this.citiesService.AddAsync(cityInput, image);

            return this.RedirectToAction("Index", "Cities", new { area = "Places" });
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.citiesService.DeleteAsync(x => x.Id == id);

            return this.RedirectToAction("Index", "Cities", new { area = "Places" });
        }

        private void SetListItems(CityInputModel city)
        {
            city.CountriesSelectListItems = this.countriesService
                                    .GetMany<CountryViewModel>(orderBySelector: x => x.Name)
                                    .Select(x => new SelectListItem(x.Name, x.Id))
                                    .ToList();
        }
    }
}
