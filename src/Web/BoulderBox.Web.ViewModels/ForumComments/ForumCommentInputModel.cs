using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.ForumComments
{
    public class ForumCommentInputModel : IMapTo<ForumComment>
    {
        [Required]
        public string ForumPostId { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(1000)]
        public string Text { get; set; }
    }
}
