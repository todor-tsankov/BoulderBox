using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Gyms
{
    public class GymInputModel : IMapTo<Gym>
    {
        public const string NameDisplay = "Name *";
        public const string CityDisplay = "City *";

        public const string InvalidNameMessage = "Name must be between 2 and 50 characters.";
        public const string InvalidDescriptionMessage = "Description can't be more than 1000 characters.";

        [Display(Name = NameDisplay)]
        [Required]
        [MinLength(2, ErrorMessage = InvalidNameMessage)]
        [MaxLength(50, ErrorMessage = InvalidNameMessage)]
        public string Name { get; set; }

        [Display(Name = CityDisplay)]
        [Required]
        public string CityId { get; set; }

        [MaxLength(1000, ErrorMessage = InvalidDescriptionMessage)]
        public string Description { get; set; }
    }
}
