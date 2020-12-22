using BoulderBox.Common;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BoulderBox.Services.CronJobs
{
    public class UpdateUserPoints
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Points> pointsRepository;
        private readonly IDeletableEntityRepository<Ascent> ascentsRepository;

        public UpdateUserPoints(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<Points> pointsRepository,
            IDeletableEntityRepository<Ascent> ascentsRepository)
        {
            this.usersRepository = usersRepository;
            this.pointsRepository = pointsRepository;
            this.ascentsRepository = ascentsRepository;
        }

        public async Task Update()
        {
            var userIds = this.usersRepository
                .All()
                .Select(x => x.Id)
                .ToArray();

            foreach (var userId in userIds)
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
