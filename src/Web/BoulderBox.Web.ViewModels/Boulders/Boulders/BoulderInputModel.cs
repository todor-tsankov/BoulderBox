using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.ViewModels.Boulders.Boulders
{
    public class BoulderInputModel : IMapTo<Boulder>
    {
        public const string NameDisplay = "Name *";
        public const string NameRequiredErrorMessage = "Name is required.";
        public const string NameLengthErrorMessage = "Name must be between 2 and 50 characters long.";

        public const string GradeIdDisplay = "Grade *";
        public const string GradeIdRequiredErrorMessage = "Grade is required.";

        public const string GymIdDisplay = "Gym *";
        public const string GymIdRequiredErrorMessage = "Gym is required.";

        public const string DescriptionDisplay = "Description *";
        public const string DescriptionRequiredErrorMessage = "Description is Required.";
        public const string DescriptionLengthErrorMessage = "Description must be between 5 and 1000 characters.";

        [Display(Name = NameDisplay)]
        [Required(ErrorMessage = NameRequiredErrorMessage)]
        [MinLength(2, ErrorMessage = NameLengthErrorMessage)]
        [MaxLength(50, ErrorMessage = NameLengthErrorMessage)]
        public string Name { get; set; }

        [Display(Name = GradeIdDisplay)]
        [Required(ErrorMessage = GradeIdRequiredErrorMessage)]
        public string GradeId { get; set; }

        [Display(Name = GymIdDisplay)]
        [Required(ErrorMessage = GymIdRequiredErrorMessage)]
        public string GymId { get; set; }

        [Display(Name = DescriptionDisplay)]
        [Required(ErrorMessage = DescriptionRequiredErrorMessage)]
        [MinLength(5, ErrorMessage = DescriptionLengthErrorMessage)]
        [MaxLength(1000, ErrorMessage = DescriptionLengthErrorMessage)]
        public string Description { get; set; }

        public IEnumerable<SelectListItem> GradesSelectItems { get; set; }

        public IEnumerable<SelectListItem> CountriesSelectItems { get; set; }
    }
}
