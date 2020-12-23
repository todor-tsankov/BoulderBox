using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Forum.Comments;

namespace BoulderBox.Services.Data.Forum
{
    public class CommentsService : BaseService<Comment>, ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;
        private readonly IMapper mapper;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository, IMapper mapper)
            : base(commentsRepository, mapper)
        {
            this.NullCheck(commentsRepository, nameof(commentsRepository));
            this.NullCheck(mapper, nameof(mapper));

            this.commentsRepository = commentsRepository;
            this.mapper = mapper;
        }

        public async Task AddAsync(CommentInputModel commentInput, string userId)
        {
            this.NullCheck(commentInput, nameof(commentInput));
            this.NullCheck(userId, nameof(userId));

            var comment = this.mapper.Map<Comment>(commentInput);

            comment.ApplicationUserId = userId;

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string id, CommentInputModel commentInput)
        {
            this.NullCheck(id, nameof(id));
            this.NullCheck(commentInput, nameof(commentInput));

            var comment = this.commentsRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            comment.Text = commentInput.Text;
            await this.commentsRepository.SaveChangesAsync();
        }
    }
}
