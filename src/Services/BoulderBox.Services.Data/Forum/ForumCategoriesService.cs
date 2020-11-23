using System.Threading.Tasks;

using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.ForumCategories;
using BoulderBox.Web.ViewModels.Images;

namespace BoulderBox.Services.Data.Forum
{
    public class ForumCategoriesService : BaseService<ForumCategory>, IForumCategoriesService
    {
        private readonly IDeletableEntityRepository<ForumCategory> forumCategoriesRepository;

        public ForumCategoriesService(IDeletableEntityRepository<ForumCategory> forumCategoriesRepository)
            : base(forumCategoriesRepository)
        {
            this.forumCategoriesRepository = forumCategoriesRepository;
        }

        public async Task Create(ForumCategoryInputModel forumCategoryInput, ImageInputModel imageInput)
        {
            var mapper = AutoMapperConfig.MapperInstance;

            var forumCategory = mapper.Map<ForumCategory>(forumCategoryInput);
            var image = mapper.Map<Image>(imageInput);

            forumCategory.Image = image;

            await this.forumCategoriesRepository.AddAsync(forumCategory);
            await this.forumCategoriesRepository.SaveChangesAsync();
        }
    }
}
