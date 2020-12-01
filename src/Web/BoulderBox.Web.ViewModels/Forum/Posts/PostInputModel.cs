using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Forum.Posts
{
    public class PostInputModel : IMapTo<Post>
    {
        [Display(Name = "Title *")]
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Display(Name = "Text *")]
        [Required(ErrorMessage = "Post' text is required.")]
        [MinLength(2, ErrorMessage = "Text must be between 2 and 10 000 characters long.")]
        [MaxLength(10000, ErrorMessage = "Text must be between 2 and 10 000 characters long.")]
        public string Text { get; set; }

        [Required]
        public string CategoryId { get; set; }
    }
}
