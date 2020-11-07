using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

namespace BoulderBox.Services.Data.Forum
{
    public class ForumPostsService : BaseService<ForumPost>, IForumPostsService
    {
        public ForumPostsService(IDeletableEntityRepository<ForumPost> forumPostsRepository)
            : base(forumPostsRepository)
        {
        }
    }
}
