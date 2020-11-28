using System.Security.Claims;
using System.Threading.Tasks;

using BoulderBox.Services.Data.Forum;
using BoulderBox.Web.ViewModels;
using BoulderBox.Web.ViewModels.ForumComments;
using BoulderBox.Web.ViewModels.ForumPosts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Controllers
{
    public class ForumPostsController : BaseController
    {
        private readonly IForumPostsService forumPostsService;
        private readonly IForumCommentsService forumCommentsService;

        public ForumPostsController(IForumPostsService forumPostsService, IForumCommentsService forumCommentsService)
        {
            this.forumPostsService = forumPostsService;
            this.forumCommentsService = forumCommentsService;
        }

        public IActionResult Details(string id, int pageId = 1)
        {
            var itemsPerPage = 5;
            var skip = itemsPerPage * (pageId - 1);

            var forumPostAndComment = new ForumPostAndCommentInputViewModel()
            {
                Username = this.User.FindFirstValue(ClaimTypes.Name),
                RedirectLink = $"{this.Request.Path}?pageId={pageId}",
                ForumPost = this.forumPostsService.GetSingle<ForumPostDetailsViewModel>(x => x.Id == id),
                ForumComments = this.forumCommentsService
                    .GetMany<ForumPostDetailsForumCommentViewModel>(
                        x => x.ForumPostId == id,
                        x => x.CreatedOn,
                        true,
                        skip,
                        itemsPerPage),
                CurrentPage = pageId,
                ItemsPerPage = itemsPerPage,
                ItemsCount = this.forumCommentsService.Count(x => x.ForumPostId == id),
            };

            return this.View(forumPostAndComment);
        }

        public IActionResult Create(string id)
        {
            var post = new ForumPostInputModel()
            {
                ForumCategoryId = id,
            };

            return this.View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ForumPostInputModel forumPostInput, IFormFile formFile)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(forumPostInput);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var image = await this.SaveImageFileAsync(formFile);

            await this.forumPostsService.Create(forumPostInput, image, userId);

            return this.Redirect($"/ForumCategories/Details/{forumPostInput.ForumCategoryId}");
        }
    }
}
