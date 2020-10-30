using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;

namespace BoulderBox.Data.Models
{
    public class Grade : BaseDeletableModel<string>
    {
        public Grade()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Ascents = new HashSet<Ascent>();
            this.Boulders = new HashSet<Boulder>();
        }

        [Required]
        public string Text { get; set; }

        public int Points { get; set; }

        public virtual ICollection<Ascent> Ascents { get; set; }

        public virtual ICollection<Boulder> Boulders { get; set; }
    }
}
