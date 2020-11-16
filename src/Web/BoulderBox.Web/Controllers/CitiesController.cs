using System.Linq;
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
        private readonly IImagesService imagesService;

        public CitiesController(ICitiesService citiesService, ICountriesService countriesService, IImagesService imagesService)
        {
            this.citiesService = citiesService;
            this.countriesService = countriesService;
            this.imagesService = imagesService;
        }

        public IActionResult Index()
        {
            var cities = this.citiesService.GetMany<CityViewModel>();

            return this.View(cities);
        }

        public IActionResult Details(string id)
        {
            var city = this.citiesService.GetSingle<CityDetailsViewModel>(x => x.Id == id);

            return this.View(city);
        }

        public IActionResult Create()
        {
            this.ViewBag.SelectListItems = this.countriesService
                .GetMany<CountryViewModel>(orderBySelector: x => x.Name)
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CityInputModel cityInput, IFormFile formFile)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
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
