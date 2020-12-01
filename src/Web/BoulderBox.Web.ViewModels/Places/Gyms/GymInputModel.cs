using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.ViewModels.Places.Gyms
{
    public class GymInputModel : IMapTo<Gym>
    {
        public const string NameDisplay = "Name *";
        public const string NameRequiredErrorMessage = "Name is required.";
        public const string NameLengthErrorMessage = "Name must be between 2 and 50 characters.";

        public const string CityIdDisplay = "City *";
        public const string CityIdRequiredErrorMessage = "City is required.";

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

        public IEnumerable<SelectListItem> CountriesSelectListItems { get; set; }
    }
}
