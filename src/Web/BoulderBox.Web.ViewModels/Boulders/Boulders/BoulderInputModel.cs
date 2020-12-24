using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AutoMapper;
using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.ViewModels.Boulders.Boulders
{
    public class BoulderInputModel : IMapTo<Boulder>, IMapFrom<Boulder>, IHaveCustomMappings
    {
        public const string NameDisplay = "Name *";
        public const string NameRequiredErrorMessage = "Name is required.";
        public const string NameLengthErrorMessage = "Name must be between 2 and 50 characters long.";

        public const string GradeIdDisplay = "Grade *";
        public const string GradeIdRequiredErrorMessage = "Grade is required.";

        public const string GymIdDisplay = "Gym *";
        public const string GymIdRequiredErrorMessage = "Gym is required.";

        public const string CountryIdDisplay = "Country *";
        public const string CountryIdRequiredErrorMessage = "Country is required.";

        public const string CityIdDisplay = "City *";
        public const string CityIdRequiredErrorMessage = "City is required.";

        public const string DescriptionDisplay = "Description *";
        public const string DescriptionRequiredErrorMessage = "Description is Required.";
        public const string DescriptionLengthErrorMessage = "Description must be between 5 and 1000 characters.";

        public const string FormFileDisplay = "Image *";
        public const string FormFileRequiredErrorMessage = "Image is required.";

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

        [Required]
        public string CountryId { get; set; }

        [Required]
        public string CityId { get; set; }

        [Display(Name = FormFileDisplay)]
        [Required(ErrorMessage = FormFileRequiredErrorMessage)]
        [ImageAttribute]
        public IFormFile FormFile { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> GradesSelectItems { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> CountriesSelectItems { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> CitiesSelectItems { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> GymsSelectItems { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Boulder, BoulderInputModel>()
                .ForMember(x => x.CityId, x => x.MapFrom(y => y.Gym.CityId))
                .ForMember(x => x.CountryId, x => x.MapFrom(y => y.Gym.City.CountryId));
        }
    }
}
