using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;
using BoulderBox.Web.ViewModels.Boulders.Ascents;

namespace BoulderBox.Services.Data.Boulders
{
    public class AscentsService : BaseService<Ascent>, IAscentsService
    {
        public const int NumberOfTopBouldersInRanking = 10;

        public const int WeeklyRankingDays = 7;
        public const int MonthlyRankingMonths = 1;
        public const int YearlyRankingYears = 1;

        private readonly IDeletableEntityRepository<Ascent> ascentsRepository;
        private readonly IDeletableEntityRepository<Points> pointsRepository;
        private readonly IMapper mapper;

        public AscentsService(
            IDeletableEntityRepository<Ascent> ascentsRepository,
            IDeletableEntityRepository<Points> pointsRepository,
            IMapper mapper)
            : base(ascentsRepository, mapper)
        {
            this.NullCheck(pointsRepository, nameof(pointsRepository));

            this.ascentsRepository = ascentsRepository;
            this.pointsRepository = pointsRepository;
            this.mapper = mapper;
        }

        public async Task AddAsync(AscentInputModel ascentInput, string userId)
        {
            this.NullCheck(ascentInput, nameof(ascentInput));
            this.NullCheck(userId, nameof(userId));

            var ascent = this.mapper.Map<Ascent>(ascentInput);
            ascent.ApplicationUserId = userId;

            var points = this.pointsRepository
                .All()
                .First(x => x.ApplicationUserId == userId);

            points.Weekly = this.CalculatePoints(userId, x => x.Date.AddDays(WeeklyRankingDays) >= DateTime.UtcNow);
            points.Monthly = this.CalculatePoints(userId, x => x.Date.AddMonths(MonthlyRankingMonths) >= DateTime.UtcNow);
            points.Yearly = this.CalculatePoints(userId, x => x.Date.AddYears(YearlyRankingYears) >= DateTime.UtcNow);
            points.AllTime = this.CalculatePoints(userId, x => true);

            await this.ascentsRepository.AddAsync(ascent);
            await this.ascentsRepository.SaveChangesAsync();
        }

        private int CalculatePoints(string userId, Expression<Func<Ascent, bool>> filter)
        {
            var points = this.ascentsRepository
                            .AllAsNoTracking()
                            .Where(x => x.ApplicationUserId == userId)
                            .Where(filter)
                            .Select(x => x.Grade.Points + x.Style.BonusPoints)
                            .OrderByDescending(x => x)
                            .Take(NumberOfTopBouldersInRanking)
                            .Sum();

            return points;
        }
    }
}
