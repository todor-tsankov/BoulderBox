using System.Threading.Tasks;

using BoulderBox.Services.Data.Forum;
using BoulderBox.Web.ViewModels.ForumCategories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Controllers
{
    public class ForumCategoriesController : BaseController
    {
        private readonly IForumCategoriesService forumCategoriesService;
        private readonly IForumPostsService forumPostsService;

        public ForumCategoriesController(IForumCategoriesService forumCategoriesService, IForumPostsService forumPostsService)
        {
            this.forumCategoriesService = forumCategoriesService;
            this.forumPostsService = forumPostsService;
        }

        public IActionResult Index()
        {
            var categories = this.forumCategoriesService
                .GetMany<ForumCategoryViewModel>();

            return this.View(categories);
        }

        public IActionResult Details(string id, int pageId = 1)
        {
            var itemsPerPage = 12;
            var skip = itemsPerPage * (pageId - 1);

            var category = this.forumCategoriesService
                .GetSingle<ForumCategoryDetailsViewModel>(x => x.Id == id);

            category.ForumPosts = this.forumPostsService
                .GetMany<ForumCategoryDetailsForumPostViewModel>(
                    x => x.ForumCategoryId == id,
                    x => x.CreatedOn,
                    false,
                    skip,
                    itemsPerPage);

            category.CurrentPage = pageId;
            category.ItemsPerPage = itemsPerPage;
            category.ItemsCount = this.forumPostsService.Count(x => x.ForumCategoryId == id);

            return this.View(category);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ForumCategoryInputModel forumCategoryInput, IFormFile formFile)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var image = await this.SaveImageFileAsync(formFile);
            await this.forumCategoriesService.Create(forumCategoryInput, image);

            return this.RedirectToAction("Index");
        }
    }
}
