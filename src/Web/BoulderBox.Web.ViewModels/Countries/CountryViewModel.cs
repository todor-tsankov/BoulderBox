using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.Cities;

namespace BoulderBox.Web.ViewModels.Countries
{
    public class CountryViewModel : IMapFrom<Country>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryCode { get; set; }

        public virtual string ImageSource { get; set; }
    }
}
