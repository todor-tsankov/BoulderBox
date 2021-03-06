﻿using System.Linq;
using System.Threading.Tasks;
using System.Web;

using BoulderBox.Common;
using BoulderBox.Services;
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
        private readonly ICloudinaryService cloudinaryService;

        public CitiesController(
            ICountriesService countriesService,
            ICitiesService citiesService,
            ICloudinaryService cloudinaryService)
        {
            this.countriesService = countriesService;
            this.citiesService = citiesService;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult Create()
        {
            var city = new CityInputModel();
            this.SetListItems(city);

            return this.View(city);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CityInputModel cityInput)
        {
            var existCountry = this.countriesService.Exists(x => x.Id == cityInput.CountryId);

            if (!this.ModelState.IsValid || !existCountry)
            {
                this.SetListItems(cityInput);
                return this.View(cityInput);
            }

            var image = await this.cloudinaryService.SaveImageAsync(cityInput.FormFile);
            await this.citiesService.AddAsync(cityInput, image);

            var cityNameEncoded = HttpUtility.HtmlEncode(cityInput.Name);
            this.TempData[GlobalConstants.MessageKey] = $"Successfully created city <strong>{cityNameEncoded}</strong>!";

            return this.RedirectToAction("Index", "Cities", new { area = "Places" });
        }

        public IActionResult Edit(string id)
        {
            var existsCity = this.citiesService
                .Exists(x => x.Id == id);

            if (!existsCity)
            {
                return this.NotFound();
            }

            var city = new CityEditViewModel()
            {
                Id = id,
                CityInput = this.citiesService
                    .GetSingle<CityInputModel>(x => x.Id == id),
            };

            this.SetListItems(city.CityInput);

            return this.View(city);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, CityInputModel cityInput)
        {
            var existsId = this.citiesService
                .Exists(x => x.Id == id);

            if (!existsId)
            {
                return this.NotFound();
            }

            var existsCity = this.citiesService
                .Exists(x => x.Id != id && x.Name == cityInput.Name && x.CountryId == cityInput.CountryId);

            if (!this.ModelState.IsValid || existsCity)
            {
                var city = new CityEditViewModel()
                {
                    Id = id,
                    CityInput = cityInput,
                };

                this.SetListItems(city.CityInput);

                return this.View(city);
            }

            var image = await this.cloudinaryService.SaveImageAsync(cityInput.FormFile);
            await this.citiesService.EditAsync(id, cityInput, image);

            var cityNameEncoded = HttpUtility.HtmlEncode(cityInput.Name);
            this.TempData[GlobalConstants.MessageKey] = $"Successfully edited city <strong>{cityNameEncoded}</strong>!";

            return this.RedirectToAction("Details", "Cities", new { area = "Places", id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var existsCity = this.citiesService
                .Exists(x => x.Id == id);

            if (!existsCity)
            {
                return this.NotFound();
            }

            await this.citiesService.DeleteAsync(x => x.Id == id);
            this.TempData[GlobalConstants.MessageKey] = $"Successfully deleted city!";

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
