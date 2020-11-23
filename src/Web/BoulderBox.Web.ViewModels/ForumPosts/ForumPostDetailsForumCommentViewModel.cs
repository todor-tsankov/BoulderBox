using System;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.ForumPosts
{
    public class ForumPostDetailsForumCommentViewModel : IMapFrom<ForumComment>
    {
        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserUserName { get; set; }

        public string Text { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
