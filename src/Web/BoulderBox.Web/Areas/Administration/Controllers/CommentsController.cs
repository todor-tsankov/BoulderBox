using System.Threading.Tasks;

using BoulderBox.Common;
using BoulderBox.Services.Data.Forum;
using BoulderBox.Web.Areas.Administration.Controllers.Common;
using BoulderBox.Web.ViewModels.Forum.Comments;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Administration.Controllers
{
    public class CommentsController : AdministrationController
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        public IActionResult Edit(string id)
        {
            var existsComment = this.commentsService
                .Exists(x => x.Id == id);

            if (!existsComment)
            {
                return this.NotFound();
            }

            var comment = new CommentEditModel()
            {
                Id = id,
                CommentInput = this.commentsService
                    .GetSingle<CommentInputModel>(x => x.Id == id),
            };

            return this.View(comment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, CommentInputModel commentInput)
        {
            var existsComment = this.commentsService
                .Exists(x => x.Id == id);

            if (!existsComment)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                var comment = new CommentEditModel()
                {
                    Id = id,
                    CommentInput = commentInput,
                };

                return this.View(comment);
            }

            await this.commentsService.EditAsync(id, commentInput);
            this.TempData[GlobalConstants.MessageKey] = $"Successfully edited comment!";

            return this.RedirectToAction("Details", "Posts", new { area = "Forum", id = commentInput.PostId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string postId)
        {
            var existsComment = this.commentsService
                .Exists(x => x.Id == id);

            if (!existsComment)
            {
                return this.NotFound();
            }

            await this.commentsService.DeleteAsync(x => x.Id == id);
            this.TempData[GlobalConstants.MessageKey] = $"Successfully deleted comment!";

            return this.RedirectToAction("Details", "Posts", new { area = "Forum", id = postId });
        }
    }
}
