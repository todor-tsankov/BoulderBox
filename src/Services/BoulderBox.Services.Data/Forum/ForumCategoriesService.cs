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
            var forumCategory = this.mapper.Map<ForumCategory>(forumCategoryInput);
            var image = this.mapper.Map<Image>(imageInput);

            forumCategory.Image = image;

            await this.forumCategoriesRepository.AddAsync(forumCategory);
            await this.forumCategoriesRepository.SaveChangesAsync();
        }
    }
}
