using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Countries
{
    public class CountryDetailsViewModel : IMapFrom<Country>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryCode { get; set; }

        public string Description { get; set; }

        public virtual string ImageSource { get; set; }

        public ICollection<CountryDetailsCityViewModel> Cities { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Country, CountryDetailsViewModel>()
                .ForMember(x => x.ImageSource, x => x.MapFrom(y => y.Image.Source))
                .ForMember(x => x.Cities, x => x.MapFrom(y => y.Cities));
        }
    }
}
