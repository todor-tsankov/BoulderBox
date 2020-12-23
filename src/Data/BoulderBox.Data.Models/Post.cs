using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;
using BoulderBox.Data.Models.Interfaces;

namespace BoulderBox.Data.Models
{
    public class Post : BaseDeletableModel<string>, IHaveImage
    {
        public Post()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Comments = new HashSet<Comment>();
        }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(10000)]
        public string Text { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string ImageId { get; set; }

        public virtual Image Image { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
