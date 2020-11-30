using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.ForumCategories;
using BoulderBox.Web.ViewModels.Images;

namespace BoulderBox.Services.Data.Forum
{
    public class ForumCategoriesService : BaseService<ForumCategory>, IForumCategoriesService
    {
        private readonly IDeletableEntityRepository<ForumCategory> forumCategoriesRepository;
        private readonly IMapper mapper;

        public ForumCategoriesService(IDeletableEntityRepository<ForumCategory> forumCategoriesRepository, IMapper mapper)
            : base(forumCategoriesRepository, mapper)
        {
            this.forumCategoriesRepository = forumCategoriesRepository;
            this.mapper = mapper;
        }

        public async Task Create(ForumCategoryInputModel forumCategoryInput, ImageInputModel imageInput)
        {
            this.NullCheck(forumCategoryInput, nameof(forumCategoryInput));

            var forumCategory = this.mapper.Map<ForumCategory>(forumCategoryInput);
            forumCategory.Image = this.mapper.Map<Image>(imageInput);

            await this.forumCategoriesRepository.AddAsync(forumCategory);
            await this.forumCategoriesRepository.SaveChangesAsync();
        }
    }
}
