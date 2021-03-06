﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.ViewModels.Places.Cities
{
    public class CityInputModel : IMapTo<City>, IMapFrom<City>
    {
        public const string NameDisplay = "Name *";
        public const string NameRequiredErrorMessage = "Name is required.";
        public const string NameLengthErrorMessage = "Name must be between 2 and 50 characters long.";

        public const string CountryIdDisplay = "Country *";
        public const string InvalidCountryIdMessage = "Country is required.";

        public const string DescriptionLengthErrorMessage = "Description can't be more than 1000 characters.";

        public const string FormFileDisplay = "Image";

        [Display(Name = NameDisplay)]
        [Required(ErrorMessage = NameRequiredErrorMessage)]
        [MinLength(2, ErrorMessage = NameLengthErrorMessage)]
        [MaxLength(50, ErrorMessage = NameLengthErrorMessage)]
        public string Name { get; set; }

        [Display(Name = CountryIdDisplay)]
        [Required(ErrorMessage = InvalidCountryIdMessage)]
        public string CountryId { get; set; }

        [MaxLength(1000, ErrorMessage = DescriptionLengthErrorMessage)]
        public string Description { get; set; }

        [Display(Name = FormFileDisplay)]
        [ImageAttribute]
        public IFormFile FormFile { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> CountriesSelectListItems { get; set; }
    }
}
