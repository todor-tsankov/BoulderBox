using System;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;

namespace BoulderBox.Data.Models
{
    public class Ascent : BaseDeletableModel<string>
    {
        public Ascent()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string BoulderId { get; set; }

        [Required]
        public virtual Boulder Boulder { get; set; }

        [Required]
        public string GradeId { get; set; }

        [Required]
        public virtual Grade Grade { get; set; }

        [Required]
        public string StyleId { get; set; }

        [Required]
        public virtual Style Style { get; set; }

        public bool Recommend { get; set; }

        public int? Stars { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; }

        public DateTime Date { get; set; }
    }
}
