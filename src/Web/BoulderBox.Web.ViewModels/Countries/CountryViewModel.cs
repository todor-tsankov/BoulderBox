using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.Cities;
using System.Collections.Generic;

namespace BoulderBox.Web.ViewModels.Countries
{
    public class CountryViewModel : IMapFrom<Country>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryCode { get; set; }

        public string Description { get; set; }

        public virtual string ImageSource { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Country, CountryViewModel>()
                .ForMember(x => x.ImageSource, x => x.MapFrom(y => y.Image.Source));
        }
    }
}
