﻿using System.Collections.Generic;
using System.Threading.Tasks;

using BoulderBox.Services.Data.Places;
using BoulderBox.Web.ViewModels.Countries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Controllers
{
    public class CountriesController : BaseController
    {
        private readonly ICountriesService countriesService;

        public CountriesController(ICountriesService countriesService)
        {
            this.countriesService = countriesService;
        }

        public IActionResult Index(int pageId = 1)
        {
            var itemsPerPage = 12;
            var skip = itemsPerPage * (pageId - 1);

            var countriesViewModel = new CountriesViewModel()
            {
                Countries = this.countriesService
                    .GetMany<CountryViewModel>(
                        orderBySelector: x => x.Name,
                        skip: skip,
                        take: itemsPerPage),
                CurrentPage = pageId,
                ItemsCount = this.countriesService.Count(),
                ItemsPerPage = itemsPerPage,
            };

            return this.View(countriesViewModel);
        }

        public IActionResult Details(string id)
        {
            var country = this.countriesService.GetSingle<CountryDetailsViewModel>(x => x.Id == id);

            return this.View(country);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CountryInputModel countryInput, IFormFile formFile)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var image = await this.SaveImageFileAsync(formFile);
            await this.countriesService.AddCountryAsync(countryInput, image);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.countriesService.DeleteAsync(x => x.Id == id);

            return this.RedirectToAction("Index");
        }
    }
}
