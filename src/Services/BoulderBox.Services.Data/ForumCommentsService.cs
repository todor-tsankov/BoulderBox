using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

namespace BoulderBox.Services.Data
{
    public class ForumCommentsService : BaseService<ForumComment>, IForumCommentsService
    {
        public ForumCommentsService(IDeletableEntityRepository<ForumComment> forumCommentsRepository)
            : base(forumCommentsRepository)
        {
        }
    }
}
