using System.Linq;
using System.Threading.Tasks;
using BoulderBox.Services.Data.Places;
using BoulderBox.Web.ViewModels.Countries;
using BoulderBox.Web.ViewModels.Gyms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.Controllers
{
    public class GymsController : BaseController
    {
        private readonly IGymsService gymsService;
        private readonly ICountriesService countriesService;

        public GymsController(IGymsService gymsService, ICountriesService countriesService)
        {
            this.gymsService = gymsService;
            this.countriesService = countriesService;
        }

        public IActionResult Index()
        {
            var gyms = this.gymsService.GetMany<GymViewModel>();

            return this.View(gyms);
        }

        public IActionResult Details()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            this.ViewBag.SelectListItems = this.countriesService
                .GetMany<CountryViewModel>(x => x.Cities.Any(), x => x.Name)
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GymInputModel gymInput, IFormFile formFile)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var image = await this.SaveImageFileAsync(formFile);
            await this.gymsService.AddGymAsync(gymInput, image);

            return this.Redirect("Index");
        }

        public IActionResult Delete(string id)
        {
            this.gymsService.Delete(x => x.Id == id);

            return this.Redirect("Index");
        }
    }
}
