using System.Linq;

using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Cities
{
    public class CityDetailsGymViewModel : IMapFrom<Gym>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int BouldersCount { get; set; }

        public int AscentsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Gym, CityDetailsGymViewModel>()
                .ForMember(x => x.AscentsCount, x => x.MapFrom(y => Sum(y.Boulders.Select(z => z.Ascents.Count).ToArray())));
        }

        private static int Sum(int[] counts)
        {
            return counts.Sum();
        }
    }
}
