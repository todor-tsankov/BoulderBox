using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

namespace BoulderBox.Services.Data
{
    public class ApplicationUsersService : BaseService<ApplicationUser>, IApplicationUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public ApplicationUsersService(IDeletableEntityRepository<ApplicationUser> userRepository)
            : base(userRepository)
        {
            this.userRepository = userRepository;
        }
    }
}
