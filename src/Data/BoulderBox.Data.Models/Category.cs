using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;
using BoulderBox.Data.Models.Interfaces;

namespace BoulderBox.Data.Models
{
    public class Category : BaseDeletableModel<string>, IHaveImage
    {
        public Category()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Posts = new HashSet<Post>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public string ImageId { get; set; }

        public virtual Image Image { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
