using System.Security.Claims;
using System.Threading.Tasks;

using BoulderBox.Common;
using BoulderBox.Services.Data.Forum;
using BoulderBox.Web.Controllers;
using BoulderBox.Web.ViewModels.Forum.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Forum.Controllers
{
    [Area("Forum")]
    public class CommentsController : BaseController
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CommentInputModel commentInput, string redirectLink)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect(redirectLink);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.commentsService.AddAsync(commentInput, userId);

            this.TempData[GlobalConstants.MessageKey] = "Successfully added comment!";

            return this.Redirect(redirectLink);
        }
    }
}
