using System.Collections.Generic;
using BoulderBox.Services.Data.Places;
using BoulderBox.Web.ViewModels.Cities;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Controllers
{
    [Route("api/Cities")]
    public class CitiesApiController : ControllerBase
    {
        private readonly ICitiesService citiesService;

        public CitiesApiController(ICitiesService citiesService)
        {
            this.citiesService = citiesService;
        }

        [HttpGet]
        public IEnumerable<CityViewModel> Get(string countryId)
        {
            var cities = this.citiesService
                .GetMany<CityViewModel>(x => x.CountryId == countryId);

            return cities;
        }
    }
}
