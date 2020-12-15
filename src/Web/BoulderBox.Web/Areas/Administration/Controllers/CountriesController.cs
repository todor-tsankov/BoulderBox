using System.Threading.Tasks;

using BoulderBox.Services;
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
        private readonly ICloudinaryService cloudinaryService;

        public CountriesController(ICountriesService countriesService, ICloudinaryService cloudinaryService)
        {
            this.countriesService = countriesService;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CountryInputModel countryInput)
        {
            var existsCountry = this.countriesService.Exists(x => x.Name == countryInput.Name);

            if (!this.ModelState.IsValid || existsCountry)
            {
                return this.View(countryInput);
            }

            var image = await this.cloudinaryService.SaveImageAsync(countryInput.FormFile);
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
        public async Task<IActionResult> Edit(string id, CountryInputModel countryInput)
        {
            var existsCountry = this.countriesService
                .Exists(x => x.Id != id && (x.Name == countryInput.Name || x.CountryCode == countryInput.CountryCode));

            if (!this.ModelState.IsValid || existsCountry)
            {
                var country = new CountryEditViewModel()
                {
                    Id = id,
                    CountryInput = countryInput,
                };

                return this.View(country);
            }

            var image = await this.cloudinaryService.SaveImageAsync(countryInput.FormFile);
            await this.countriesService.EditAsync(id, countryInput, image);

            return this.RedirectToAction("Index", "Countries", new { area = "Places" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await this.countriesService.DeleteAsync(x => x.Id == id);

            return this.RedirectToAction("Index", "Countries", new { area = "Places" });
        }
    }
}
