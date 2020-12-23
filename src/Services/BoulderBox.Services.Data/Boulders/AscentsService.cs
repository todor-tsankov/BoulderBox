using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Common;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Boulders.Ascents;

namespace BoulderBox.Services.Data.Boulders
{
    public class AscentsService : BaseService<Ascent>, IAscentsService
    {
        private readonly IDeletableEntityRepository<Ascent> ascentsRepository;
        private readonly IDeletableEntityRepository<Points> pointsRepository;
        private readonly IMapper mapper;

        public AscentsService(
            IDeletableEntityRepository<Ascent> ascentsRepository,
            IDeletableEntityRepository<Points> pointsRepository,
            IMapper mapper)
            : base(ascentsRepository, mapper)
        {
            this.NullCheck(mapper, nameof(mapper));
            this.NullCheck(ascentsRepository, nameof(ascentsRepository));
            this.NullCheck(pointsRepository, nameof(pointsRepository));

            this.mapper = mapper;
            this.ascentsRepository = ascentsRepository;
            this.pointsRepository = pointsRepository;
        }

        public async Task AddAsync(AscentInputModel ascentInput, string userId)
        {
            this.NullCheck(ascentInput, nameof(ascentInput));
            this.NullCheck(userId, nameof(userId));

            var ascent = this.mapper.Map<Ascent>(ascentInput);
            ascent.ApplicationUserId = userId;

            await this.ascentsRepository.AddAsync(ascent);
            await this.ascentsRepository.SaveChangesAsync();
            await this.CalculatePointsForUser(userId);
        }

        public async Task EditAsync(string id, AscentInputModel ascentInput)
        {
            this.NullCheck(id, nameof(id));
            this.NullCheck(ascentInput, nameof(ascentInput));

            var ascent = this.ascentsRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            ascent.GradeId = ascentInput.GradeId;
            ascent.Recommend = ascentInput.Recommend;
            ascent.Stars = ascentInput.Stars;
            ascent.StyleId = ascentInput.StyleId;
            ascent.Comment = ascentInput.Comment;
            ascent.Date = ascentInput.Date;

            await this.ascentsRepository.SaveChangesAsync();
            await this.CalculatePointsForUser(ascent.ApplicationUserId);
        }

        private async Task CalculatePointsForUser(string userId)
        {
            var points = this.pointsRepository
                            .All()
                            .First(x => x.ApplicationUserId == userId);

            points.Weekly = this.CalculatePoints(userId, x => x.Date.AddDays(GlobalConstants.WeeklyRankingDays) >= DateTime.UtcNow);
            points.Monthly = this.CalculatePoints(userId, x => x.Date.AddMonths(GlobalConstants.MonthlyRankingMonths) >= DateTime.UtcNow);
            points.Yearly = this.CalculatePoints(userId, x => x.Date.AddYears(GlobalConstants.YearlyRankingYears) >= DateTime.UtcNow);
            points.AllTime = this.CalculatePoints(userId, x => true);

            await this.pointsRepository.SaveChangesAsync();
        }

        private int CalculatePoints(string userId, Expression<Func<Ascent, bool>> filter)
        {
            var points = this.ascentsRepository
                            .AllAsNoTracking()
                            .Where(x => x.ApplicationUserId == userId)
                            .Where(filter)
                            .Select(x => x.Grade.Points + x.Style.BonusPoints)
                            .OrderByDescending(x => x)
                            .Take(GlobalConstants.NumberOfTopBouldersInRanking)
                            .Sum();

            return points;
        }
    }
}
