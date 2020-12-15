using System.Linq;
using System.Threading.Tasks;

using BoulderBox.Services;
using BoulderBox.Services.Data.Boulders;
using BoulderBox.Services.Data.Places;
using BoulderBox.Web.Areas.Administration.Controllers.Common;
using BoulderBox.Web.ViewModels.Boulders.Boulders;
using BoulderBox.Web.ViewModels.Boulders.Grades;
using BoulderBox.Web.ViewModels.Places.Cities;
using BoulderBox.Web.ViewModels.Places.Countries;
using BoulderBox.Web.ViewModels.Places.Gyms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.Areas.Administration.Controllers
{
    public class BouldersController : AdministrationController
    {
        private readonly IBouldersService bouldersService;
        private readonly ICountriesService countriesService;
        private readonly ICitiesService citiesService;
        private readonly IGymsService gymsService;
        private readonly IGradesService gradesService;
        private readonly ICloudinaryService cloudinaryService;

        public BouldersController(
            IBouldersService bouldersService,
            ICountriesService countriesService,
            ICitiesService citiesService,
            IGymsService gymsService,
            IGradesService gradesService,
            ICloudinaryService cloudinaryService)
        {
            this.bouldersService = bouldersService;
            this.countriesService = countriesService;
            this.citiesService = citiesService;
            this.gymsService = gymsService;
            this.gradesService = gradesService;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult Edit(string id)
        {
            var boulder = new BoulderEditViewModel()
            {
                Id = id,
                BoulderInput = this.bouldersService
                    .GetSingle<BoulderInputModel>(x => x.Id == id),
            };

            this.SetSelectListItems(boulder.BoulderInput);

            return this.View(boulder);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, BoulderInputModel boulderInput)
        {
            if (!this.ModelState.IsValid)
            {
                var boulder = new BoulderEditViewModel()
                {
                    Id = id,
                    BoulderInput = boulderInput,
                };

                this.SetSelectListItems(boulder.BoulderInput);

                return this.View(boulder);
            }

            var image = await this.cloudinaryService.SaveImageAsync(formFile);
            await this.bouldersService.EditAsync(id, boulderInput, image);

            return this.RedirectToAction("Index", "Boulders", new { area = "Boulders" });
        }

        private void SetSelectListItems(BoulderInputModel boulder)
        {
            boulder.CountriesSelectItems = this.countriesService
                            .GetMany<CountryViewModel>(x => x.Cities.Any(y => y.Gyms.Any()), x => x.Name)
                            .Select(x => new SelectListItem()
                            {
                                Value = x.Id,
                                Text = x.Name,
                            })
                            .ToList();

            boulder.CitiesSelectItems = this.citiesService
                .GetMany<CityViewModel>(x => x.CountryId == boulder.CountryId && x.Gyms.Any(), x => x.Name)
                .Select(x => new SelectListItem()
                {
                    Value = x.Id,
                    Text = x.Name,
                })
                .ToList();

            boulder.GymsSelectItems = this.gymsService
               .GetMany<GymViewModel>(x => x.CityId == boulder.CityId, x => x.Name)
               .Select(x => new SelectListItem()
               {
                   Value = x.Id,
                   Text = x.Name,
               })
               .ToList();

            boulder.GradesSelectItems = this.gradesService
                .GetMany<GradeViewModel>(orderBySelector: x => x.Text)
                .Select(x => new SelectListItem()
                {
                    Value = x.Id,
                    Text = x.Text,
                })
                .ToList();
        }
    }
}
