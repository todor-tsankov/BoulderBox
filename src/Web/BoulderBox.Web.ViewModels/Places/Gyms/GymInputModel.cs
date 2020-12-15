using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.ViewModels.Places.Gyms
{
    public class GymInputModel : IMapTo<Gym>, IMapFrom<Gym>, IHaveCustomMappings
    {
        public const string NameDisplay = "Name *";
        public const string NameRequiredErrorMessage = "Name is required.";
        public const string NameLengthErrorMessage = "Name must be between 2 and 50 characters.";

        public const string CityIdDisplay = "City *";
        public const string CityIdRequiredErrorMessage = "City is required.";

        public const string CountryIdDisplay = "Country *";
        public const string CountryIdRequiredErrorMessage = "Country is required.";

        public const string InvalidDescriptionMessage = "Description can't be more than 1000 characters.";

        [Display(Name = NameDisplay)]
        [Required(ErrorMessage = NameRequiredErrorMessage)]
        [MinLength(2, ErrorMessage = NameLengthErrorMessage)]
        [MaxLength(50, ErrorMessage = NameLengthErrorMessage)]
        public string Name { get; set; }

        [Display(Name = CityIdDisplay)]
        [Required(ErrorMessage = CityIdRequiredErrorMessage)]
        public string CityId { get; set; }

        [MaxLength(1000, ErrorMessage = InvalidDescriptionMessage)]
        public string Description { get; set; }

        [Display(Name = CountryIdDisplay)]
        [Required(ErrorMessage = CountryIdRequiredErrorMessage)]
        public string CountryId { get; set; }

        [ImageAttribute]
        public IFormFile FormFile { get; set; }

        public IEnumerable<SelectListItem> CountriesSelectListItems { get; set; }

        public IEnumerable<SelectListItem> CitiesSelectListItems { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Gym, GymInputModel>()
                .ForMember(x => x.CountryId, x => x.MapFrom(y => y.City.CountryId));
        }
    }
}
