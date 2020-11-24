﻿using System.Collections.Generic;

using BoulderBox.Web.ViewModels.ForumComments;

namespace BoulderBox.Web.ViewModels.ForumPosts
{
    public class ForumPostAndCommentInputViewModel
    {
        public string Username { get; set; }

        public string RedirectLink { get; set; }

        public ForumPostDetailsViewModel ForumPost { get; set; }

        public IEnumerable<ForumPostDetailsForumCommentViewModel> ForumComments { get; set; }

        public ForumCommentInputModel ForumComment { get; set; }
    }
}