using System;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;

namespace BoulderBox.Data.Models
{
    public class Comment : BaseDeletableModel<string>
    {
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(10000)]
        public string Text { get; set; }

        [Required]
        public string PostId { get; set; }

        [Required]
        public virtual Post Post { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
