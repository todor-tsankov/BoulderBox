using System;
using System.Linq.Expressions;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Places;
using BoulderBox.Web.Controllers;
using BoulderBox.Web.ViewModels.Common;
using BoulderBox.Web.ViewModels.Places.Cities;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Places.Controllers
{
    [Area("Places")]
    public class CitiesController : BaseController
    {
        private readonly ICitiesService citiesService;

        public CitiesController(ICitiesService citiesService)
        {
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
            var existsCity = this.citiesService
                .Exists(x => x.Id == id);

            if (!existsCity)
            {
                return this.NotFound();
            }

            var city = this.citiesService
                .GetSingle<CityDetailsViewModel>(x => x.Id == id);

            return this.View(city);
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
