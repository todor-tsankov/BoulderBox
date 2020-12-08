﻿using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Places.Countries
{
    public class CountryInputModel : IMapTo<Country>
    {
        public const string NameDisplay = "Name *";
        public const string NameRequiredErrorMessage = "Name is required.";
        public const string NameLengthErrorMessage = "Name must be between 2 and 50 characters long.";

        public const string CountryCodeRegex = @"[A-Z]{3}";
        public const string CountryCodeDisplay = "Country Code *";
        public const string CountryCodeRequiredErrorMessage = "Country Code is required.";
        public const string CountryCodeInvalidErrorMessage = "Country Code must consist of three capital letters.";

        [Display(Name = NameDisplay)]
        [Required(ErrorMessage = NameRequiredErrorMessage)]
        [MinLength(2, ErrorMessage = NameLengthErrorMessage)]
        [MaxLength(50, ErrorMessage = NameLengthErrorMessage)]
        public string Name { get; set; }

        [Display(Name = CountryCodeDisplay)]
        [Required(ErrorMessage = CountryCodeRequiredErrorMessage)]
        [RegularExpression(CountryCodeRegex, ErrorMessage = CountryCodeInvalidErrorMessage)]
        public string CountryCode { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
    }
}