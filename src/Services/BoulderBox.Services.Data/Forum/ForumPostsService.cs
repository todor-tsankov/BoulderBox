using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.ForumPosts;
using BoulderBox.Web.ViewModels.Images;

namespace BoulderBox.Services.Data.Forum
{
    public class ForumPostsService : BaseService<ForumPost>, IForumPostsService
    {
        private readonly IDeletableEntityRepository<ForumPost> forumPostsRepository;
        private readonly IMapper mapper;

        public ForumPostsService(IDeletableEntityRepository<ForumPost> forumPostsRepository, IMapper mapper)
            : base(forumPostsRepository, mapper)
        {
            this.forumPostsRepository = forumPostsRepository;
            this.mapper = mapper;
        }

        public async Task Create(ForumPostInputModel forumPostInput, ImageInputModel imageInput, string userId)
        {
            this.NullCheck(forumPostInput, nameof(forumPostInput));
            this.NullCheck(userId, nameof(userId));

            var image = this.mapper.Map<Image>(imageInput);
            var forumPost = this.mapper.Map<ForumPost>(forumPostInput);

            forumPost.Image = image;
            forumPost.ApplicationUserId = userId;

            await this.forumPostsRepository.AddAsync(forumPost);
            await this.forumPostsRepository.SaveChangesAsync();
        }
    }
}
