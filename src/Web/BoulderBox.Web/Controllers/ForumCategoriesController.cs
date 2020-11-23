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

        public ForumCategoriesController(IForumCategoriesService forumCategoriesService)
        {
            this.forumCategoriesService = forumCategoriesService;
        }

        public IActionResult Index()
        {
            var categories = this.forumCategoriesService
                .GetMany<ForumCategoryViewModel>();

            return this.View(categories);
        }

        public IActionResult Details(string id)
        {
            var category = this.forumCategoriesService
                .GetSingle<ForumCategoryDetailsViewModel>(x => x.Id == id);

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
