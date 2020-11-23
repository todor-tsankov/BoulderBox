using System.Threading.Tasks;

using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.ForumComments;

namespace BoulderBox.Services.Data.Forum
{
    public class ForumCommentsService : BaseService<ForumComment>, IForumCommentsService
    {
        private readonly IDeletableEntityRepository<ForumComment> forumCommentsRepository;

        public ForumCommentsService(IDeletableEntityRepository<ForumComment> forumCommentsRepository)
            : base(forumCommentsRepository)
        {
            this.forumCommentsRepository = forumCommentsRepository;
        }

        public async Task Create(ForumCommentInputModel forumCommentInput, string userId)
        {
            var mapper = AutoMapperConfig.MapperInstance;
            var comment = mapper.Map<ForumComment>(forumCommentInput);

            comment.ApplicationUserId = userId;

            await this.forumCommentsRepository.AddAsync(comment);
            await this.forumCommentsRepository.SaveChangesAsync();
        }
    }
}
