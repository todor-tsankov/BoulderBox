using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.ValidationAttributes;
using Microsoft.AspNetCore.Http;

namespace BoulderBox.Web.ViewModels.Forum.Categories
{
    public class CategoryInputModel : IMapTo<Category>, IMapFrom<Category>
    {
        public const string NameDisplay = "Name *";
        public const string NameRequiredErrorMessage = "Name is required.";
        public const string NameLengthErrorMessage = "Name must be between 2 and 50 characters long.";

        public const string DescriptionLengthErrorMessage = "Description can't be more than 1000 characters long.";

        public const string FormFileDisplay = "Image";

        [Display(Name = NameDisplay)]
        [Required(ErrorMessage = NameRequiredErrorMessage)]
        [MinLength(2, ErrorMessage = NameLengthErrorMessage)]
        [MaxLength(50, ErrorMessage = NameLengthErrorMessage)]
        public string Name { get; set; }

        [MaxLength(1000, ErrorMessage = DescriptionLengthErrorMessage)]
        public string Description { get; set; }

        [Display(Name = FormFileDisplay)]
        [ImageAttribute]
        public IFormFile FormFile { get; set; }
    }
}
