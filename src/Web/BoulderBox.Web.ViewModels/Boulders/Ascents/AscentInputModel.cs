using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.ViewModels.Boulders.Ascents
{
    public class AscentInputModel : IMapTo<Ascent>, IMapFrom<Ascent>
    {
        public const string GradeIdDisplay = "Grade *";
        public const string GradeIdRequiredErrorMessage = "Grade is required.";

        public const string StyleIdDisplay = "Style *";
        public const string StyleIdRequiredErrorMessage = "Style is required.";

        public const string DateDisplay = "Date *";
        public const string DateRequiredErrorMessage = "Date is required.";

        public const string StarsDisplay = "Stars (1-5)";
        public const string RecommendDisplay = "Recommend *";

        [Required]
        public string BoulderId { get; set; }

        [Display(Name = GradeIdDisplay)]
        [Required(ErrorMessage = GradeIdRequiredErrorMessage)]
        public string GradeId { get; set; }

        [Display(Name = StyleIdDisplay)]
        [Required(ErrorMessage = StyleIdRequiredErrorMessage)]
        public string StyleId { get; set; }

        public bool Recommend { get; set; }

        [Display(Name = StarsDisplay)]
        [Range(1, 5)]
        public int? Stars { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; }

        [Display(Name = DateDisplay)]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = DateRequiredErrorMessage)]
        [AscentDateAtribute]
        public DateTime Date { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> GradesSelectListItems { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> StylesSelectListItems { get; set; }
    }
}
