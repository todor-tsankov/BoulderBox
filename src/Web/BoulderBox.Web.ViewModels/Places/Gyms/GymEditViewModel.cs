using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Places.Gyms
{
    public class GymEditViewModel
    {
        [IgnoreMap]
        public string Id { get; set; }

        public GymInputModel GymInput { get; set; }
    }
}
