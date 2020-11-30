using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.ForumComments;

namespace BoulderBox.Services.Data.Forum
{
    public class ForumCommentsService : BaseService<ForumComment>, IForumCommentsService
    {
        private readonly IDeletableEntityRepository<ForumComment> forumCommentsRepository;
        private readonly IMapper mapper;

        public ForumCommentsService(IDeletableEntityRepository<ForumComment> forumCommentsRepository, IMapper mapper)
            : base(forumCommentsRepository, mapper)
        {
            this.forumCommentsRepository = forumCommentsRepository;
            this.mapper = mapper;
        }

        public async Task Create(ForumCommentInputModel forumCommentInput, string userId)
        {
            this.NullCheck(forumCommentInput, nameof(forumCommentInput));
            this.NullCheck(userId, nameof(userId));

            var comment = this.mapper.Map<ForumComment>(forumCommentInput);

            comment.ApplicationUserId = userId;

            await this.forumCommentsRepository.AddAsync(comment);
            await this.forumCommentsRepository.SaveChangesAsync();
        }
    }
}
