using System.Threading.Tasks;

using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.ForumPosts;
using BoulderBox.Web.ViewModels.Images;

namespace BoulderBox.Services.Data.Forum
{
    public class ForumPostsService : BaseService<ForumPost>, IForumPostsService
    {
        private readonly IDeletableEntityRepository<ForumPost> forumPostsRepository;

        public ForumPostsService(IDeletableEntityRepository<ForumPost> forumPostsRepository)
            : base(forumPostsRepository)
        {
            this.forumPostsRepository = forumPostsRepository;
        }

        public async Task Create(ForumPostInputModel forumPostInput, ImageInputModel imageInput, string userId)
        {
            this.NullCheck(forumPostInput, nameof(forumPostInput));
            this.NullCheck(userId, nameof(userId));

            var mapper = AutoMapperConfig.MapperInstance;

            var image = mapper.Map<Image>(imageInput);
            var forumPost = mapper.Map<ForumPost>(forumPostInput);

            forumPost.Image = image;
            forumPost.ApplicationUserId = userId;

            await this.forumPostsRepository.AddAsync(forumPost);
            await this.forumPostsRepository.SaveChangesAsync();
        }
    }
}
