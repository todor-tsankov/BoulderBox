using System.Collections.Generic;
using System.Linq;

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
        public IEnumerable<CityViewModel> Get(string countryId)
        {
            var cities = this.citiesService
                .GetMany<CityViewModel>(x => x.CountryId == countryId);

            return cities;
        }
    }
}
