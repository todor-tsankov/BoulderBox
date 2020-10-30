using System;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;

namespace BoulderBox.Data.Models
{
    public class ForumComment : BaseDeletableModel<string>
    {
        public ForumComment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string ForumPostId { get; set; }

        public virtual ForumPost ForumPost { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }
    }
}
