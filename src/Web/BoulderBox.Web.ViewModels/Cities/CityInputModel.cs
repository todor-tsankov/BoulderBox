using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Cities
{
    public class CityInputModel : IMapTo<City>
    {
        public const string NameDisplay = "Name *";
        public const string CountryIdDisplay = "Country *";

        public const string InvalidNameMessage = "City's Name must be between 2 and 50 characters long.";
        public const string InvalidCountryIdMessage = "City's country is required.";

        [Display(Name = NameDisplay)]
        [Required]
        [MinLength(2, ErrorMessage = InvalidNameMessage)]
        [MaxLength(50, ErrorMessage = InvalidNameMessage)]
        public string Name { get; set; }

        [Display(Name = CountryIdDisplay)]
        [Required(ErrorMessage = InvalidCountryIdMessage)]
        public string CountryId { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
