using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;
using BoulderBox.Data.Models.Interfaces;

namespace BoulderBox.Data.Models
{
    public class City : BaseDeletableModel<string>, IHaveImage
    {
        public City()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Gyms = new HashSet<Gym>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string CountryId { get; set; }

        [Required]
        public virtual Country Country { get; set; }

        public string ImageId { get; set; }

        public virtual Image Image { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public virtual ICollection<Gym> Gyms { get; set; }
    }
}
