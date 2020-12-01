using System.Collections.Generic;

using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Users.Users;

namespace BoulderBox.Services.Data.Users
{
    public interface IApplicationUsersService : IBaseService<ApplicationUser>
    {
        IEnumerable<AscentGroupViewModel> GetGrouped(string userId);
    }
}
