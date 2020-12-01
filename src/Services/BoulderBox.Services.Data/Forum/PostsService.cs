using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Forum.Posts;

namespace BoulderBox.Services.Data.Forum
{
    public class PostsService : BaseService<Post>, IPostsService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IMapper mapper;

        public PostsService(IDeletableEntityRepository<Post> postsRepository, IMapper mapper)
            : base(postsRepository, mapper)
        {
            this.postsRepository = postsRepository;
            this.mapper = mapper;
        }

        public async Task Create(PostInputModel postInput, ImageInputModel imageInput, string userId)
        {
            this.NullCheck(postInput, nameof(postInput));
            this.NullCheck(userId, nameof(userId));

            var post = this.mapper.Map<Post>(postInput);
            post.Image = this.mapper.Map<Image>(imageInput);

            post.ApplicationUserId = userId;

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();
        }
    }
}
