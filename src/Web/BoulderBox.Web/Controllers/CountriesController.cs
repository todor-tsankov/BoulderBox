using System.Collections.Generic;
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

        public IActionResult Index()
        {
            var countries = this.countriesService.GetMany<CountryViewModel>();

            return this.View(countries);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CountryInputModel input, IFormFile formFile)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var image = await this.SaveImageFileAsync(formFile);
            await this.countriesService.AddCountryAsync(input, image);

            return this.RedirectToAction("Index");
        }
    }
}
