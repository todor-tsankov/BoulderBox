using System.Threading.Tasks;

using BoulderBox.Services.Data.Places;
using BoulderBox.Web.Areas.Administration.Controllers.Common;
using BoulderBox.Web.ViewModels.Places.Countries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Administration.Controllers
{
    public class CountriesController : AdministrationController
    {
        private readonly ICountriesService countriesService;

        public CountriesController(ICountriesService countriesService)
        {
            this.countriesService = countriesService;
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

            return this.RedirectToAction("Index", "Countries", new { area = "Places" });
        }

        public IActionResult Edit(string id)
        {
            var country = new CountryEditViewModel()
            {
                Id = id,
                CountryInput = this.countriesService
                    .GetSingle<CountryInputModel>(x => x.Id == id),
            };

            return this.View(country);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, CountryInputModel countryInput, IFormFile formFile)
        {
            var existsCountry = this.countriesService
                .Exists(x => x.Id != id && (x.Name == countryInput.Name || x.CountryCode == countryInput.CountryCode));

            if (!this.ModelState.IsValid || existsCountry)
            {
                return this.View(countryInput);
            }

            var image = await this.SaveImageFileAsync(formFile);
            await this.countriesService.EditAsync(id, countryInput, image);

            return this.RedirectToAction("Index", "Countries", new { area = "Places" });
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.countriesService.DeleteAsync(x => x.Id == id);

            return this.RedirectToAction("Index", "Countries", new { area = "Places" });
        }
    }
}
