using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels.Users.Users;
using Microsoft.EntityFrameworkCore;

namespace BoulderBox.Services.Data.Users
{
    public class ApplicationUsersService : BaseService<ApplicationUser>, IApplicationUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Ascent> ascentsRepository;
        private readonly IMapper mapper;

        public ApplicationUsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<Ascent> ascentsRepository,
            IMapper mapper)
            : base(usersRepository, mapper)
        {
            this.usersRepository = usersRepository;
            this.ascentsRepository = ascentsRepository;
            this.mapper = mapper;
        }

        public IEnumerable<AscentGroupViewModel> GetGrouped(string userId)
        {
            var groups = this.ascentsRepository
                .AllAsNoTracking()
                .Include(x => x.Boulder)
                .Include(x => x.Boulder.Image)
                .Include(x => x.Grade)
                .Include(x => x.Style)
                .Where(x => x.ApplicationUserId == userId)
                .ToList()
                .GroupBy(x => x.Grade.Text)
                .OrderByDescending(x => x.Key)
                .Select(x => new AscentGroupViewModel()
                {
                    Key = x.Key,
                    Ascents = x.ToList().Select(x => new UserDetailsAscentViewModel()
                    {
                        Id = x.Id,
                        BoulderImageSource = x.Boulder.Image.Source,
                        GradeText = x.Grade.Text,
                        Stars = x.Stars,
                        StyleShortText = x.Style.ShortText,
                        BoulderId = x.BoulderId,
                        BoulderName = x.Boulder.Name,
                        Comment = x.Comment,
                        Date = x.Date,
                    }),
                })
                .ToArray();

            return groups;
        }
    }
}
