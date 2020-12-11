using System.Security.Claims;
using System.Threading.Tasks;

using BoulderBox.Services.Data.Forum;
using BoulderBox.Web.Controllers;
using BoulderBox.Web.ViewModels;
using BoulderBox.Web.ViewModels.Common;
using BoulderBox.Web.ViewModels.Forum.Comments;
using BoulderBox.Web.ViewModels.Forum.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Forum.Controllers
{
    [Area("Forum")]
    public class PostsController : BaseController
    {
        private readonly IPostsService postsService;
        private readonly ICommentsService commentsService;

        public PostsController(IPostsService postsService, ICommentsService commentsService)
        {
            this.postsService = postsService;
            this.commentsService = commentsService;
        }

        public IActionResult Details(string id, int pageId = 1)
        {
            var skip = DefaultItemsPerPage * (pageId - 1);

            var postAndComment = new PostAndCommentInputViewModel()
            {
                Username = this.User.FindFirstValue(ClaimTypes.Name),
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
            var post = new PostInputModel()
            {
                CategoryId = id,
            };

            return this.View(post);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(PostInputModel postInput, IFormFile formFile)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(postInput);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var image = await this.SaveImageFileAsync(formFile);

            await this.postsService.AddAsync(postInput, image, userId);

            return this.RedirectToAction("Details", "Categories", new { id = postInput.CategoryId });
        }
    }
}
