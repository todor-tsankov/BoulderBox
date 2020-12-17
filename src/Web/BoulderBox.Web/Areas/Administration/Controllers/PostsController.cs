using System.Linq;
using System.Threading.Tasks;

using BoulderBox.Common;
using BoulderBox.Services;
using BoulderBox.Services.Data.Forum;
using BoulderBox.Web.Areas.Administration.Controllers.Common;
using BoulderBox.Web.ViewModels.Forum.Categories;
using BoulderBox.Web.ViewModels.Forum.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoulderBox.Web.Areas.Administration.Controllers
{
    public class PostsController : AdministrationController
    {
        private readonly IPostsService postsService;
        private readonly ICategoriesService categoriesService;
        private readonly ICloudinaryService cloudinaryService;

        public PostsController(
            IPostsService postsService,
            ICategoriesService categoriesService,
            ICloudinaryService cloudinaryService)
        {
            this.postsService = postsService;
            this.categoriesService = categoriesService;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult Edit(string id)
        {
            var existsPost = this.postsService
                .Exists(x => x.Id == id);

            if (!existsPost)
            {
                return this.NotFound();
            }

            var post = new PostEditModel()
            {
                Id = id,
                PostInput = this.postsService
                    .GetSingle<PostInputModel>(x => x.Id == id),
            };

            this.SetSelectListItems(post);

            return this.View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, PostInputModel postInput)
        {
            var existsPost = this.postsService
                .Exists(x => x.Id == id);

            if (!existsPost)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                var post = new PostEditModel()
                {
                    Id = id,
                    PostInput = postInput,
                };

                this.SetSelectListItems(post);

                return this.View(post);
            }

            var image = await this.cloudinaryService.SaveImageAsync(postInput.FormFile);

            await this.postsService.EditAsync(id, postInput, image);
            this.TempData[GlobalConstants.MessageKey] = $"Successfully edited post!";

            return this.RedirectToAction("Details", "Posts", new { area = "Forum", id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var existsPost = this.postsService
                .Exists(x => x.Id == id);

            if (!existsPost)
            {
                return this.NotFound();
            }

            await this.postsService.DeleteAsync(x => x.Id == id);
            this.TempData[GlobalConstants.MessageKey] = $"Successfully deleted post!";

            return this.RedirectToAction("Index", "Categories", new { area = "Forum" });
        }

        private void SetSelectListItems(PostEditModel post)
        {
            post.PostInput.CategoriesSelectListItems = this.categoriesService
                            .GetMany<CategoryViewModel>()
                            .Select(x => new SelectListItem(x.Name, x.Id));
        }
    }
}
