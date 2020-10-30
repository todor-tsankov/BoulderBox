using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;
using BoulderBox.Data.Models.Interfaces;

namespace BoulderBox.Data.Models
{
    public class Gym : BaseDeletableModel<string>, IHaveImage
    {
        public Gym()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Boulders = new HashSet<Boulder>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string CityId { get; set; }

        [Required]
        public virtual City City { get; set; }

        public string ImageId { get; set; }

        public virtual Image Image { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public virtual ICollection<Boulder> Boulders { get; set; }
    }
}
