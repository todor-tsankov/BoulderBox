using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Places;
using BoulderBox.Web.ViewModels.Places.Cities;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Places.Controllers
{
    [Area("Api")]
    [Route("api/Cities")]
    [ApiController]
    public class CitiesApiController : ControllerBase
    {
        private readonly ICitiesService citiesService;

        public CitiesApiController(ICitiesService citiesService)
        {
            this.citiesService = citiesService;
        }

        [HttpGet]
        public IEnumerable<CityViewModel> Get(string countryId, bool withGyms)
        {
            Expression<Func<City, bool>> predicate = x => x.CountryId == countryId;

            if (withGyms)
            {
                predicate = x => x.CountryId == countryId && x.Gyms.Any();
            }

            var cities = this.citiesService
                .GetMany<CityViewModel>(predicate);

            return cities;
        }
    }
}
