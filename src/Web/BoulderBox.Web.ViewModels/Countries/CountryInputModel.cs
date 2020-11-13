using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Countries
{
    public class CountryInputModel : IMapTo<Country>
    {
        public const string NameDisplay = "Name *";
        public const string CountryCodeDisplay = "Country Code *";

        public const string NameLengthErrorMessage = "Country Name must be between 2 and 50 characters long.";

        public const string CountryCodeRegex = @"[A-Z]{3}";
        public const string CountryCodeErrorMessage = @"Country code must consist of three capital letters";

        [Display(Name = NameDisplay)]
        [Required]
        [MinLength(2, ErrorMessage = NameLengthErrorMessage)]
        [MaxLength(50, ErrorMessage = NameLengthErrorMessage)]
        public string Name { get; set; }

        [Display(Name = CountryCodeDisplay)]
        [Required]
        [RegularExpression(CountryCodeRegex, ErrorMessage = CountryCodeErrorMessage)]
        public string CountryCode { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
