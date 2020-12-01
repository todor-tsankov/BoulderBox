using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Places.Countries
{
    public class CountryViewModel : IMapFrom<Country>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryCode { get; set; }

        public virtual string ImageSource { get; set; }
    }
}
