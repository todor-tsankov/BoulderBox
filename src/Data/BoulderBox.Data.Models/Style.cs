using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;

namespace BoulderBox.Data.Models
{
    public class Style : BaseDeletableModel<string>
    {
        public Style()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Ascents = new HashSet<Ascent>();
        }

        [Required]
        [MaxLength(2)]
        public string ShortText { get; set; }

        [Required]
        [MaxLength(20)]
        public string LongText { get; set; }

        public int BonusPoints { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public virtual ICollection<Ascent> Ascents { get; set; }
    }
}
