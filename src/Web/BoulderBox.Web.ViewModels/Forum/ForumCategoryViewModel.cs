using BoulderBox.Data.Models;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Web.ViewModels.Forum
{
    public class ForumCategoryViewModel : IMapFrom<ForumCategory>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int ForumPostsCount { get; set; }
    }
}
