using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.ForumComments;

namespace BoulderBox.Services.Data.Forum
{
    public interface IForumCommentsService : IBaseService<ForumComment>
    {
        Task Create(ForumCommentInputModel forumCommentInput, string userId);
    }
}
