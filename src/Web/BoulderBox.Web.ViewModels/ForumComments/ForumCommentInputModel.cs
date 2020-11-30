﻿using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.ForumComments
{
    public class ForumCommentInputModel : IMapTo<ForumComment>
    {
        public const string ForumPostIdRequiredErrorMessage = "ForumPost is required.";

        public const string TextRequiredErrorMessage = "Text is required.";
        public const string TextLengthErrorMessage = "Text must be between 2 and 10000 characters.";

        [Required(ErrorMessage = ForumPostIdRequiredErrorMessage)]
        public string ForumPostId { get; set; }

        [Required(ErrorMessage = TextRequiredErrorMessage)]
        [MinLength(2, ErrorMessage = TextLengthErrorMessage)]
        [MaxLength(10000, ErrorMessage = TextLengthErrorMessage)]
        public string Text { get; set; }
    }
}
