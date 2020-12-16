using System;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Forum.Posts
{
    public class PostDetailsForumCommentViewModel : IMapFrom<Comment>
    {
        public string ApplicationUserId { get; set; }

        public string ApplicationUserUserName { get; set; }

        public string ApplicationUserImageSource { get; set; }

        public string Text { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
