﻿using System.Linq;
using System.Threading.Tasks;
using System.Web;

using BoulderBox.Common;
using BoulderBox.Services;
using BoulderBox.Services.Data.Places;
using BoulderBox.Web.Areas.Administration.Controllers.Common;
using BoulderBox.Web.ViewModels.Places.Cities;
using BoulderBox.Web.ViewModels.Places.Countries;
using BoulderBox.Web.ViewModels.Places.Gyms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.Areas.Administration.Controllers
{
    public class GymsController : AdministrationController
    {
        private readonly ICountriesService countriesService;
        private readonly ICitiesService citiesService;
        private readonly IGymsService gymsService;
        private readonly ICloudinaryService cloudinaryService;

        public GymsController(
            ICountriesService countriesService,
            ICitiesService citiesService,
            IGymsService gymsService,
            ICloudinaryService cloudinaryService)
        {
            this.countriesService = countriesService;
            this.citiesService = citiesService;
            this.gymsService = gymsService;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult Create()
        {
            var gym = new GymInputModel();
            this.SetCountrySelectListItems(gym);
            return this.View(gym);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GymInputModel gymInput)
        {
            var cityExists = this.citiesService.Exists(x => x.Id == gymInput.CityId);

            if (!this.ModelState.IsValid || !cityExists)
            {
                this.SetCountrySelectListItems(gymInput);

                return this.View(gymInput);
            }

            var image = await this.cloudinaryService.SaveImageAsync(gymInput.FormFile);
            await this.gymsService.AddAsync(gymInput, image);

            var encodedGymName = HttpUtility.HtmlEncode(gymInput.Name);
            this.TempData[GlobalConstants.MessageKey] = $"Successfully created gym <strong>{encodedGymName}</strong>!";

            return this.RedirectToAction("Index", "Gyms", new { area = "Places" });
        }

        public IActionResult Edit(string id)
        {
            var existsGym = this.gymsService
                .Exists(x => x.Id == id);

            if (!existsGym)
            {
                return this.NotFound();
            }

            var gym = new GymEditViewModel()
            {
                Id = id,
                GymInput = this.gymsService
                    .GetSingle<GymInputModel>(x => x.Id == id),
            };

            this.SetCountrySelectListItems(gym.GymInput);
            this.SetCitySelectListItems(gym.GymInput);

            return this.View(gym);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, GymInputModel gymInput)
        {
            var existsGym = this.gymsService
                .Exists(x => x.Id == id);

            if (!existsGym)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                var gym = new GymEditViewModel()
                {
                    Id = id,
                    GymInput = gymInput,
                };

                this.SetCountrySelectListItems(gym.GymInput);
                this.SetCitySelectListItems(gym.GymInput);

                return this.View(gym);
            }

            var image = await this.cloudinaryService.SaveImageAsync(gymInput.FormFile);
            await this.gymsService.EditAsync(id, gymInput, image);

            var encodedGymName = HttpUtility.HtmlEncode(gymInput.Name);
            this.TempData[GlobalConstants.MessageKey] = $"Successfully edited gym <strong>{encodedGymName}</strong>!";

            return this.RedirectToAction("Details", "Gyms", new { area = "Places", id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var existsGym = this.gymsService
                .Exists(x => x.Id == id);

            if (!existsGym)
            {
                return this.NotFound();
            }

            await this.gymsService.DeleteAsync(x => x.Id == id);
            this.TempData[GlobalConstants.MessageKey] = $"Successfully deleted gym!";

            return this.RedirectToAction("Index", "Gyms", new { area = "Places" });
        }

        private void SetCountrySelectListItems(GymInputModel gym)
        {
            gym.CountriesSelectListItems = this.countriesService
                                .GetMany<CountryViewModel>(x => x.Cities.Any(), x => x.Name)
                                .Select(x => new SelectListItem(x.Name, x.Id))
                                .ToList();
        }

        private void SetCitySelectListItems(GymInputModel gym)
        {
            gym.CitiesSelectListItems = this.citiesService
                                .GetMany<CityViewModel>(x => x.CountryId == gym.CountryId)
                                .Select(x => new SelectListItem(x.Name, x.Id))
                                .ToList();
        }
    }
}
