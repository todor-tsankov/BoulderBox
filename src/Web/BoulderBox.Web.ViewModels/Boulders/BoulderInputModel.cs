using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.ViewModels.Boulders
{
    public class BoulderInputModel : IMapTo<Boulder>
    {
        public const string NameDisplay = "Name *";
        public const string GradeIdDisplay = "Grade *";
        public const string GymIdDisplay = "Gym *";

        public const string InvalidNameMessage = "Name must be between 2 and 50 characters long.";
        public const string InvalidGradeIdMessage = "Grade is required.";
        public const string InvalidNameGymIdMessage = "Gym is required.";

        [Display(Name = NameDisplay)]
        [Required(ErrorMessage = InvalidNameMessage)]
        [MinLength(2, ErrorMessage = InvalidNameMessage)]
        [MaxLength(50, ErrorMessage = InvalidNameMessage)]
        public string Name { get; set; }

        [Display(Name = GradeIdDisplay)]
        [Required(ErrorMessage = InvalidGradeIdMessage)]
        public string GradeId { get; set; }

        [Display(Name = GymIdDisplay)]
        [Required(ErrorMessage = InvalidNameGymIdMessage)]
        public string GymId { get; set; }

        public string AuthorId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        public IEnumerable<SelectListItem> GradesSelectItems { get; set; }

        public IEnumerable<SelectListItem> CountriesSelectItems { get; set; }
    }
}
