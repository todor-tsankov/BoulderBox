using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

namespace BoulderBox.Services.Data.Forum
{
    public class ForumCategoriesService : BaseService<ForumCategory>, IForumCategoriesService
    {
        public ForumCategoriesService(IDeletableEntityRepository<ForumCategory> forumCategoriesRepository)
            : base(forumCategoriesRepository)
        {
        }
    }
}
