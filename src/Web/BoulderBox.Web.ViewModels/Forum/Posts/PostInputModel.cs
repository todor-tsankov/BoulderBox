using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.ViewModels.Forum.Posts
{
    public class PostInputModel : IMapTo<Post>, IMapFrom<Post>
    {
        public const string TitleDisplay = "Title *";
        public const string TitleRequiredErrorMessage = "Title is required.";
        public const string TitleLengthErrorMessage = "Title must be between 2 and 50 characters long.";

        public const string TextDisplay = "Text *";
        public const string TextRequiredErrorMessage = "Post's text is required.";
        public const string TextLengthErrorMessage = "Text must be between 2 and 10 000 characters long.";

        public const string FormFileDisplay = "Image";

        public const string CategoryIdDisplay = "Category *";
        public const string CategoryIdrequiredErrorMessage = "Category is required.";

        [Display(Name = TitleDisplay)]
        [Required(ErrorMessage = TitleRequiredErrorMessage)]
        [MinLength(2, ErrorMessage = TitleLengthErrorMessage)]
        [MaxLength(50, ErrorMessage = TitleLengthErrorMessage)]
        public string Title { get; set; }

        [Display(Name = TextDisplay)]
        [Required(ErrorMessage = TextRequiredErrorMessage)]
        [MinLength(2, ErrorMessage = TextLengthErrorMessage)]
        [MaxLength(10000, ErrorMessage = TextLengthErrorMessage)]
        public string Text { get; set; }

        [Display(Name = CategoryIdDisplay)]
        [Required(ErrorMessage = CategoryIdrequiredErrorMessage)]
        public string CategoryId { get; set; }

        [Display(Name = FormFileDisplay)]
        [ImageAttribute]
        public IFormFile FormFile { get; set; }

        public IEnumerable<SelectListItem> CategoriesSelectListItems { get; set; }
    }
}
