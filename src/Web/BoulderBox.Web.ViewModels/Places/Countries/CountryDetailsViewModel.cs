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
        public string Name { get; set; }

        public string CountryCode { get; set; }

        public string Description { get; set; }

        public virtual string ImageSource { get; set; }

        public ICollection<CountryDetailsCityViewModel> Cities { get; set; }
    }
}
