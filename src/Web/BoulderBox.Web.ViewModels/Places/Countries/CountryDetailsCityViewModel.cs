using System.Collections.Generic;
using System.Linq;

using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Places.Countries
{
    public class CountryDetailsCityViewModel : IMapFrom<City>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ImageSource { get; set; }

        public int GymsCount { get; set; }

        public int BouldersCount { get; set; }

        public int AscentsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<City, CountryDetailsCityViewModel>()
                .ForMember(x => x.BouldersCount, x => x.MapFrom(y => Sum(y.Gyms.Select(z => z.Boulders.Count).ToArray())))
                .ForMember(x => x.AscentsCount, x => x.MapFrom(y => Sum(y.Gyms.SelectMany(z => z.Boulders).Select(z => z.Ascents.Count).ToArray())));
        }

        private static int Sum(int[] counts)
        {
            return counts.Sum();
        }
    }
}
