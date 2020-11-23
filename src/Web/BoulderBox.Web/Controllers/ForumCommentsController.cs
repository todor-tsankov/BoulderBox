using System.Security.Claims;
using System.Threading.Tasks;

using BoulderBox.Services.Data.Forum;
using BoulderBox.Web.ViewModels.ForumComments;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Controllers
{
    public class ForumCommentsController : BaseController
    {
        private readonly IForumCommentsService forumCommentsService;

        public ForumCommentsController(IForumCommentsService forumCommentsService)
        {
            this.forumCommentsService = forumCommentsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ForumCommentInputModel forumCommentInput, string redirectLink)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect(redirectLink);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.forumCommentsService.Create(forumCommentInput, userId);

            return this.Redirect(redirectLink);
        }
    }
}
