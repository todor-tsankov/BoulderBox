using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

namespace BoulderBox.Services.Data.Users
{
    public class ApplicationUsersService : BaseService<ApplicationUser>, IApplicationUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IMapper mapper;

        public ApplicationUsersService(IDeletableEntityRepository<ApplicationUser> userRepository, IMapper mapper)
            : base(userRepository, mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
    }
}
