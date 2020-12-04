﻿using System.Collections.Generic;
using System.Threading.Tasks;

using BoulderBox.Services.Data.Places;
using BoulderBox.Web.Controllers;
using BoulderBox.Web.ViewModels.Common;
using BoulderBox.Web.ViewModels.Places.Countries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Places.Controllers
{
    [Area("Places")]
    public class CountriesController : BaseController
    {
        private readonly ICountriesService countriesService;

        public CountriesController(ICountriesService countriesService)
        {
            this.countriesService = countriesService;
        }

        public IActionResult Index(int pageId = 1)
        {
            var skip = DefaultItemsPerPage * (pageId - 1);

            var countriesViewModel = new CountriesViewModel()
            {
                Countries = this.countriesService
                    .GetMany<CountryViewModel>(
                        orderBySelector: x => x.Name,
                        skip: skip,
                        take: DefaultItemsPerPage),

                Pagination = this.GetPaginationModel(pageId, this.countriesService.Count()),
            };

            return this.View(countriesViewModel);
        }

        public IActionResult Details(string id)
        {
            var country = this.countriesService
                .GetSingle<CountryDetailsViewModel>(x => x.Id == id);

            return this.View(country);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CountryInputModel countryInput, IFormFile formFile)
        {
            var existsCountry = this.countriesService.Exists(x => x.Name == countryInput.Name);

            if (!this.ModelState.IsValid || existsCountry)
            {
                return this.View(countryInput);
            }

            var image = await this.SaveImageFileAsync(formFile);
            await this.countriesService.AddAsync(countryInput, image);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.countriesService.DeleteAsync(x => x.Id == id);

            return this.RedirectToAction("Index");
        }
    }
}
