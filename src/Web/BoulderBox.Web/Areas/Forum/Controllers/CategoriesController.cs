using System.Threading.Tasks;

using BoulderBox.Services.Data.Forum;
using BoulderBox.Web.Controllers;
using BoulderBox.Web.ViewModels.Common;
using BoulderBox.Web.ViewModels.Forum.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Forum.Controllers
{
    [Area("Forum")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IPostsService postsService;

        public CategoriesController(ICategoriesService categoriesService, IPostsService postsService)
        {
            this.categoriesService = categoriesService;
            this.postsService = postsService;
        }

        public IActionResult Index()
        {
            var categories = this.categoriesService
                .GetMany<CategoryViewModel>();

            return this.View(categories);
        }

        public IActionResult Details(string id, int pageId = 1)
        {
            var existsCategory = this.categoriesService
                .Exists(x => x.Id == id);

            if (!existsCategory)
            {
                return this.NotFound();
            }

            var skip = DefaultItemsPerPage * (pageId - 1);

            var category = this.categoriesService
                .GetSingle<CategoryDetailsViewModel>(x => x.Id == id);

            category.Posts = this.postsService
                .GetMany<CategoryDetailsForumPostViewModel>(
                    x => x.CategoryId == id,
                    x => x.CreatedOn,
                    false,
                    skip,
                    DefaultItemsPerPage);

            category.Common = new CommonViewModel()
            {
                Pagination = this.GetPaginationModel(pageId, this.postsService.Count(x => x.CategoryId == id)),
            };

            return this.View(category);
        }
    }
}
