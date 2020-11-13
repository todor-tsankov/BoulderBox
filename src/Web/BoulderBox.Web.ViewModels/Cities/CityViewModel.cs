using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Cities
{
    public class CityViewModel : IMapFrom<City>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryName { get; set; }

        public string ImageSource { get; set; }

        public string Description { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<City, CityViewModel>()
                .ForMember(x => x.CountryName, x => x.MapFrom(y => y.Country.Name))
                .ForMember(x => x.ImageSource, x => x.MapFrom(y => y.Image.Source));
        }
    }
}
