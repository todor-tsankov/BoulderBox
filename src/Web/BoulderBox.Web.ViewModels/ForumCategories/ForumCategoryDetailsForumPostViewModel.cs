using System;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.ForumCategories
{
    public class ForumCategoryDetailsForumPostViewModel : IMapFrom<ForumPost>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserUserName { get; set; }

        public string ImageSource { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ForumCommentsCount { get; set; }
    }
}
