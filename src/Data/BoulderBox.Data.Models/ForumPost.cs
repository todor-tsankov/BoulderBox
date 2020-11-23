using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;
using BoulderBox.Data.Models.Interfaces;

namespace BoulderBox.Data.Models
{
    public class ForumPost : BaseDeletableModel<string>, IHaveImage
    {
        public ForumPost()
        {
            this.Id = Guid.NewGuid().ToString();

            this.ForumComments = new HashSet<ForumComment>();
        }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(10000)]
        public string Text { get; set; }

        public string ForumCategoryId { get; set; }

        public ForumCategory ForumCategory { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string ImageId { get; set; }

        public virtual Image Image { get; set; }

        public virtual ICollection<ForumComment> ForumComments { get; set; }
    }
}
