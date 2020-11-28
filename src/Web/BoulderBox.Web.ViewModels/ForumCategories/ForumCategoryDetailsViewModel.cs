using System.Collections.Generic;

using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.Common;

namespace BoulderBox.Web.ViewModels.ForumCategories
{
    public class ForumCategoryDetailsViewModel : PaginationViewModel, IMapFrom<ForumCategory>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageSource { get; set; }

        [AutoMapper.IgnoreMap]
        public IEnumerable<ForumCategoryDetailsForumPostViewModel> ForumPosts { get; set; }
    }
}
