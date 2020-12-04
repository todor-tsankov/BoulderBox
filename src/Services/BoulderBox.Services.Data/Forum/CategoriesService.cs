using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Forum.Categories;

namespace BoulderBox.Services.Data.Forum
{
    public class CategoriesService : BaseService<Category>, ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IMapper mapper;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepository, IMapper mapper)
            : base(categoriesRepository, mapper)
        {
            this.categoriesRepository = categoriesRepository;
            this.mapper = mapper;
        }

        public async Task AddAsync(CategoryInputModel categoryInput, ImageInputModel imageInput)
        {
            this.NullCheck(categoryInput, nameof(categoryInput));

            var category = this.mapper.Map<Category>(categoryInput);
            category.Image = this.mapper.Map<Image>(imageInput);

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();
        }
    }
}
