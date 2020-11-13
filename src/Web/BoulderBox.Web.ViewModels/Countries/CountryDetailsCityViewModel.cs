using System.Linq;

using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Countries
{
    public class CountryDetailsCityViewModel : IMapFrom<City>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryName { get; set; }

        public string ImageSource { get; set; }

        public string Description { get; set; }

        public int GymsCount { get; set; }

        public int BoulderCount { get; set; }

        public int AscentsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<City, CountryDetailsCityViewModel>()
                .ForMember(x => x.CountryName, x => x.MapFrom(y => y.Country.Name))
                .ForMember(x => x.ImageSource, x => x.MapFrom(y => y.Image.Source))
                .ForMember(x => x.GymsCount, x => x.MapFrom(y => y.Gyms.Count))
                .ForMember(x => x.BoulderCount, x => x.MapFrom(y => y.Gyms.Sum(z => z.Boulders.Count)))
                .ForMember(x => x.AscentsCount, x => x.MapFrom(y => y.Gyms.SelectMany(z => z.Boulders).Sum(z => z.Ascents.Count)));
        }
    }
}
