using System.Security.Claims;
using System.Threading.Tasks;

using BoulderBox.Common;
using BoulderBox.Data.Models;
using BoulderBox.Services;
using BoulderBox.Services.Data.Files;
using BoulderBox.Services.Data.Forum;
using BoulderBox.Web.Controllers;
using BoulderBox.Web.ViewModels;
using BoulderBox.Web.ViewModels.Common;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Forum.Comments;
using BoulderBox.Web.ViewModels.Forum.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Forum.Controllers
{
    [Area("Forum")]
    public class PostsController : BaseController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IPostsService postsService;
        private readonly ICommentsService commentsService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IImagesService imagesService;

        public PostsController(
            ICategoriesService categoriesService,
            IPostsService postsService,
            ICommentsService commentsService,
            ICloudinaryService cloudinaryService,
            UserManager<ApplicationUser> userManager,
            IImagesService imagesService)
        {
            this.categoriesService = categoriesService;
            this.postsService = postsService;
            this.commentsService = commentsService;
            this.cloudinaryService = cloudinaryService;
            this.userManager = userManager;
            this.imagesService = imagesService;
        }

        [Authorize]
        public async Task<IActionResult> Details(string id, int pageId = 1)
        {
            var existsPost = this.postsService
                .Exists(x => x.Id == id);

            if (!existsPost)
            {
                return this.NotFound();
            }

            var skip = DefaultItemsPerPage * (pageId - 1);
            var user = await this.userManager.GetUserAsync(this.User);
            var image = this.imagesService.GetSingle<ImageViewModel>(x => x.Id == user.ImageId);

            var postAndComment = new PostAndCommentInputViewModel()
            {
                Username = user.UserName,
                ImageSource = image?.Source,
                RedirectLink = $"{this.Request.Path}?pageId={pageId}",
                Post = this.postsService.GetSingle<PostDetailsViewModel>(x => x.Id == id),
                Comments = this.commentsService
                    .GetMany<PostDetailsForumCommentViewModel>(
                        x => x.PostId == id,
                        x => x.CreatedOn,
                        true,
                        skip,
                        DefaultItemsPerPage),

                Common = new CommonViewModel()
                {
                    Pagination = this.GetPaginationModel(pageId, this.commentsService.Count(x => x.PostId == id)),
                },
            };

            return this.View(postAndComment);
        }

        [Authorize]
        public IActionResult Create(string id)
        {
            var existsCategory = this.categoriesService
                .Exists(x => x.Id == id);

            if (!existsCategory)
            {
                return this.NotFound();
            }

            var post = new PostInputModel()
            {
                CategoryId = id,
            };

            return this.View(post);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(PostInputModel postInput)
        {
            var existsCategory = this.categoriesService
                .Exists(x => x.Id == postInput.CategoryId);

            if (!existsCategory)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(postInput);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var image = await this.cloudinaryService.SaveImageAsync(postInput.FormFile);
            await this.postsService.AddAsync(postInput, image, userId);

            this.TempData[GlobalConstants.MessageKey] = "Successfully created post!";

            return this.RedirectToAction("Details", "Categories", new { id = postInput.CategoryId });
        }
    }
}
