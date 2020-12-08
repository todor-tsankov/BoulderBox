using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Files;
using BoulderBox.Services.Data.Places;
using BoulderBox.Web.Controllers;
using BoulderBox.Web.ViewModels.Common;
using BoulderBox.Web.ViewModels.Places.Cities;
using BoulderBox.Web.ViewModels.Places.Countries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.Areas.Places.Controllers
{
    [Area("Places")]
    public class CitiesController : BaseController
    {
        private readonly ICitiesService citiesService;
        private readonly ICountriesService countriesService;

        public CitiesController(ICitiesService citiesService, ICountriesService countriesService)
        {
            this.citiesService = citiesService;
            this.countriesService = countriesService;
        }

        public IActionResult Index(SortingInputModel sorting, int pageId = 1)
        {
            if (sorting.OrderBy == null)
            {
                sorting = new SortingInputModel()
                {
                    Ascending = true,
                    OrderBy = "Name",
                };
            }

            var orderBySelector = GetOrderBySelector(sorting);
            var skip = DefaultItemsPerPage * (pageId - 1);

            var citiesViewModel = new CitiesViewModel()
            {
                Cities = this.citiesService
                    .GetMany<CityViewModel>(
                        orderBySelector: orderBySelector,
                        asc: sorting.Ascending,
                        skip: skip,
                        take: DefaultItemsPerPage),

                Common = new CommonViewModel()
                {
                    Pagination = this.GetPaginationModel(pageId, this.citiesService.Count()),
                    Sorting = sorting,
                },
            };

            return this.View(citiesViewModel);
        }

        public IActionResult Details(string id)
        {
            var city = this.citiesService
                .GetSingle<CityDetailsViewModel>(x => x.Id == id);

            return this.View(city);
        }

        public IActionResult Create()
        {
            var city = new CityInputModel();
            this.SetListItems(city);

            return this.View(city);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CityInputModel cityInput, IFormFile formFile)
        {
            var existCountry = this.countriesService.Exists(x => x.Id == cityInput.CountryId);

            if (!this.ModelState.IsValid || !existCountry)
            {
                this.SetListItems(cityInput);
                return this.View(cityInput);
            }

            var image = await this.SaveImageFileAsync(formFile);
            await this.citiesService.AddAsync(cityInput, image);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.citiesService.DeleteAsync(x => x.Id == id);

            return this.RedirectToAction("Index");
        }

        private void SetListItems(CityInputModel city)
        {
            city.CountriesSelectListItems = this.countriesService
                                    .GetMany<CountryViewModel>(orderBySelector: x => x.Name)
                                    .Select(x => new SelectListItem(x.Name, x.Id))
                                    .ToList();
        }

        private static Expression<Func<City, object>> GetOrderBySelector(SortingInputModel sortingModel)
        {
            Expression<Func<City, object>> orderBySelect;

            orderBySelect = sortingModel.OrderBy switch
            {
                "Name" => x => x.Name,
                "GymsCount" => x => x.Gyms.Count,
                _ => x => x.Name,
            };

            return orderBySelect;
        }
    }
}
