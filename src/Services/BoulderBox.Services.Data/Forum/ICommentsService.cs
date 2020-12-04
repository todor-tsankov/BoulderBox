using System.Threading.Tasks;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Forum.Comments;

namespace BoulderBox.Services.Data.Forum
{
    public interface ICommentsService : IBaseService<Comment>
    {
        Task AddAsync(CommentInputModel commentInput, string userId);
    }
}
