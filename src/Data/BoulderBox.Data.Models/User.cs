using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BoulderBox.Data.Models
{
    public class User
    {
        public User()
            : base()
        {
            this.Ascents = new HashSet<Ascent>();
            this.Boulders = new HashSet<Boulder>();
            this.ForumPosts = new HashSet<ForumPost>();
            this.ForumComments = new HashSet<ForumComment>();
        }

        [Required]
        public string PointsId { get; set; }

        [Required]
        public virtual Points Points { get; set; }

        [MaxLength(1000)]
        public string Bio { get; set; }

        public virtual ICollection<Ascent> Ascents { get; set; }

        public virtual ICollection<Boulder> Boulders { get; set; }

        public virtual ICollection<ForumPost> ForumPosts { get; set; }

        public virtual ICollection<ForumComment> ForumComments { get; set; }
    }
}
