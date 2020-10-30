using System;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;

namespace BoulderBox.Data.Models
{
    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Source { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
