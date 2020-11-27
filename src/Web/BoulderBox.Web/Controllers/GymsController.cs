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

        public IActionResult Index(int pageId = 1)
        {
            var itemsPerPage = 12;
            var skip = itemsPerPage * (pageId - 1);

            var gymsViewModel = new GymsViewModel()
            {
                Gyms = this.gymsService
                    .GetMany<GymViewModel>(
                        orderBySelector: x => x.Name,
                        skip: skip,
                        take: itemsPerPage),
                CurrentPage = pageId,
                ItemsCount = this.gymsService.Count(),
                ItemsPerPage = itemsPerPage,
            };

            return this.View(gymsViewModel);
        }

        public IActionResult Details(string id)
        {
            var gym = this.gymsService.GetSingle<GymDetailsViewModel>(x => x.Id == id);

            return this.View(gym);
        }

        public IActionResult Create()
        {
            var gym = new GymInputModel();

            gym.CountriesSelectListItems = this.countriesService
                .GetMany<CountryViewModel>(x => x.Cities.Any(), x => x.Name)
                .Select(x => new SelectListItem(x.Name, x.Id))
                .ToList();

            return this.View(gym);
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

        public async Task<IActionResult> Delete(string id)
        {
            await this.gymsService.DeleteAsync(x => x.Id == id);

            return this.RedirectToAction("Index");
        }
    }
}
