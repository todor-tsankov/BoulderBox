using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoulderBox.Data.Models;
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
            return this.View();
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CountryInputModel input, List<FormFile> formFiles)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.countriesService.AddCountryAsync(input);

            return this.RedirectToAction("Index");
        }
    }
}
