using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Cities
{
    public class CityViewModel : IMapFrom<City>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryId { get; set; }

        public string CountryName { get; set; }

        public string ImageSource { get; set; }

        public string Description { get; set; }
    }
}
