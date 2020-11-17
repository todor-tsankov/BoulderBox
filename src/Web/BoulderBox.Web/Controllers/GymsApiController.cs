using System.Collections.Generic;

using BoulderBox.Services.Data.Places;
using BoulderBox.Web.ViewModels.Gyms;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Controllers
{
    [Route("api/Gyms")]
    public class GymsApiController : ControllerBase
    {
        private readonly IGymsService gymsService;

        public GymsApiController(IGymsService gymsService)
        {
            this.gymsService = gymsService;
        }

        [HttpGet]
        public IEnumerable<GymViewModel> Get(string cityId)
        {
            var gyms = this.gymsService
                .GetMany<GymViewModel>(x => x.CityId == cityId);

            return gyms;
        }
    }
}
