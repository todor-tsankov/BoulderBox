using System.Linq;
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

        public async Task EditAsync(string id, CategoryInputModel categoryInput, ImageInputModel imageInput)
        {
            this.NullCheck(id, nameof(id));
            this.NullCheck(categoryInput, nameof(categoryInput));

            var category = this.categoriesRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (imageInput != null)
            {
                category.Image = this.mapper.Map<Image>(imageInput);
            }

            category.Name = categoryInput.Name;
            category.Description = categoryInput.Description;

            await this.categoriesRepository.SaveChangesAsync();
        }
    }
}
