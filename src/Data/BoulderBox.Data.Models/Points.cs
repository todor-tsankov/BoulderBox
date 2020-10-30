using System;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;

namespace BoulderBox.Data.Models
{
    public class Points : BaseDeletableModel<string>
    {
        public Points()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int Weekly { get; set; }

        public int Monthly { get; set; }

        public int Yearly { get; set; }

        public int AllTime { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
