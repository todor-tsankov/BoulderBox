using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Places.Countries
{
    public class CountryDetailsViewModel : IMapFrom<Country>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryCode { get; set; }

        public string Description { get; set; }

        public virtual string ImageSource { get; set; }

        [IgnoreMap]
        public IEnumerable<CountryDetailsCityViewModel> Cities { get; set; }
    }
}
