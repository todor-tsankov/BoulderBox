using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;
using BoulderBox.Data.Models.Interfaces;

namespace BoulderBox.Data.Models
{
    public class Country : BaseDeletableModel<string>, IHaveImage
    {
        public Country()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Cities = new HashSet<City>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(3)]
        public string CountryCode { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public string ImageId { get; set; }

        public virtual Image Image { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
