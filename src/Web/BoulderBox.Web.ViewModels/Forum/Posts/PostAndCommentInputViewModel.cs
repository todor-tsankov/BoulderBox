using System.Collections.Generic;

using BoulderBox.Web.ViewModels.Common;
using BoulderBox.Web.ViewModels.Forum.Comments;

namespace BoulderBox.Web.ViewModels.Forum.Posts
{
    public class PostAndCommentInputViewModel
    {
        public CommonViewModel Common { get; set; }

        public string Username { get; set; }

        public string RedirectLink { get; set; }

        public PostDetailsViewModel Post { get; set; }

        public IEnumerable<PostDetailsForumCommentViewModel> Comments { get; set; }

        public CommentInputModel CommentInput { get; set; }
    }
}
