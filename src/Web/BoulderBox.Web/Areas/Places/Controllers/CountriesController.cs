using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Models;
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
        private readonly ICitiesService citiesService;

        public CountriesController(ICountriesService countriesService, ICitiesService citiesService)
        {
            this.countriesService = countriesService;
            this.citiesService = citiesService;
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

            var countriesViewModel = new CountriesViewModel()
            {
                Countries = this.countriesService
                    .GetMany<CountryViewModel>(
                        orderBySelector: orderBySelector,
                        asc: sorting.Ascending,
                        skip: skip,
                        take: DefaultItemsPerPage),

                Common = new CommonViewModel()
                {
                    Pagination = this.GetPaginationModel(pageId, this.countriesService.Count()),
                    Sorting = sorting,
                },
            };

            return this.View(countriesViewModel);
        }

        public IActionResult Details(string id)
        {
            var existsCountry = this.countriesService
                .Exists(x => x.Id == id);

            if (!existsCountry)
            {
                return this.NotFound();
            }

            var country = this.countriesService
                .GetSingle<CountryDetailsViewModel>(x => x.Id == id);

            country.Cities = this.citiesService
                .GetMany<CountryDetailsCityViewModel>(x => x.CountryId == id, x => x.Name);

            return this.View(country);
        }

        private static Expression<Func<Country, object>> GetOrderBySelector(SortingInputModel sortingModel)
        {
            Expression<Func<Country, object>> orderBySelect;

            orderBySelect = sortingModel.OrderBy switch
            {
                "Name" => x => x.Name,
                "CountryCode" => x => x.CountryCode,
                "CitiesCount" => x => x.Cities.Count,
                _ => x => x.Name,
            };

            return orderBySelect;
        }
    }
}
