using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;
using BoulderBox.Data.Models.Interfaces;

namespace BoulderBox.Data.Models
{
    public class Boulder : BaseDeletableModel<string>, IHaveImage
    {
        public Boulder()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Ascents = new HashSet<Ascent>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public virtual ApplicationUser Author { get; set; }

        [Required]
        public string GymId { get; set; }

        [Required]
        public virtual Gym Gym { get; set; }

        [Required]
        public string GradeId { get; set; }

        [Required]
        public virtual Grade Grade { get; set; }

        public string ImageId { get; set; }

        public virtual Image Image { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public virtual ICollection<Ascent> Ascents { get; set; }
    }
}
