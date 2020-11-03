// ReSharper disable VirtualMemberCallInConstructor
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BoulderBox.Data.Common.Models;
using BoulderBox.Data.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BoulderBox.Data.Models
{
    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity, IHaveImage
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Points = new Points { ApplicationUserId = this.Id };

            this.Ascents = new HashSet<Ascent>();
            this.Boulders = new HashSet<Boulder>();
            this.ForumPosts = new HashSet<ForumPost>();
            this.ForumComments = new HashSet<ForumComment>();

            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        [Required]
        public string PointsId { get; set; }

        [Required]
        public virtual Points Points { get; set; }

        [MaxLength(1000)]
        public string Bio { get; set; }

        public string ImageId { get; set; }

        public virtual Image Image { get; set; }

        public virtual ICollection<Ascent> Ascents { get; set; }

        public virtual ICollection<Boulder> Boulders { get; set; }

        public virtual ICollection<ForumPost> ForumPosts { get; set; }

        public virtual ICollection<ForumComment> ForumComments { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
