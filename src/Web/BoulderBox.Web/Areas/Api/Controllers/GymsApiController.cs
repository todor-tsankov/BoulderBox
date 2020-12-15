using System.Collections.Generic;
using System.Web;

using BoulderBox.Services.Data.Places;
using BoulderBox.Web.ViewModels.Places.Gyms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Places.Controllers
{
    [Authorize]
    [Area("Api")]
    [Route("api/Gyms")]
    [ApiController]
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

            foreach (var gym in gyms)
            {
                gym.Name = HttpUtility.HtmlEncode(gym.Name);
                gym.CityName = HttpUtility.HtmlEncode(gym.CityName);
            }

            return gyms;
        }
    }
}
