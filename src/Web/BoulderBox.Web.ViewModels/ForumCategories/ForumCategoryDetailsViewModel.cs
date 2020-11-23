using System.Collections.Generic;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.ForumCategories
{
    public class ForumCategoryDetailsViewModel : IMapFrom<ForumCategory>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageSource { get; set; }

        public ICollection<ForumCategoryDetailsForumPostViewModel> ForumPosts { get; set; }
    }
}
