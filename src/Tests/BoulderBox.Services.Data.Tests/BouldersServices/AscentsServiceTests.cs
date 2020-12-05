using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Boulders;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels;
using BoulderBox.Web.ViewModels.Boulders.Ascents;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.BouldersServices
{
    public class AscentsServiceTests
    {
        [Fact]
        public void AscentsServiceThrowsIfPointsRepositoryIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var ascentsRepoMock = new Mock<IDeletableEntityRepository<Ascent>>();

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                new AscentsService(ascentsRepoMock.Object, null, mapperMock.Object);
            });
        }

        [Fact]
        public async Task AddAsyncThrowsWhenTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var ascentsRepoMock = new Mock<IDeletableEntityRepository<Ascent>>();
            var pointsRepoMock = new Mock<IDeletableEntityRepository<Points>>();

            var ascentsService = new AscentsService(ascentsRepoMock.Object, pointsRepoMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await ascentsService.AddAsync(null, "validUserId");
            });
        }

        [Fact]
        public async Task AddAsyncThrowsWhenTheUserIdIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var ascentsRepoMock = new Mock<IDeletableEntityRepository<Ascent>>();
            var pointsRepoMock = new Mock<IDeletableEntityRepository<Points>>();

            var ascentsService = new AscentsService(ascentsRepoMock.Object, pointsRepoMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await ascentsService.AddAsync(new AscentInputModel(), null);
            });
        }

        [Theory]
        [InlineData("userId1", "boulderId1", "styleId1", "gradeId1", 0, null, false, 1, 150, 150, 150, 150)]
        [InlineData("userId2", "boulderId2", "styleId2", "gradeId2", 1, "", true, 15, 0, 150, 150, 150)]
        [InlineData("userId3", "boulderId3", "styleId3", "gradeId3", 2, "comment3", false, 100, 0, 0, 150, 150)]
        [InlineData("userId4", "boulderId4", "styleId4", "gradeId4", 3, "comment4", true, 500, 0, 0, 0, 150)]
        public async Task AddAsyncAddsTheEntityAndSetsItsPoints(
            string userId,
            string boulderId,
            string styleId,
            string gradeId,
            int stars,
            string comment,
            bool recommend,
            int daysToSubtract,
            int weeklyPoints,
            int monthlyPoints,
            int yearlyPoints,
            int allTimePoints)
        {
            var timeSpan = TimeSpan.FromDays(daysToSubtract);
            var ascentDate = DateTime.UtcNow.Subtract(timeSpan);

            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);

            var ascentsRepoMock = new Mock<IDeletableEntityRepository<Ascent>>();
            var pointsRepoMock = new Mock<IDeletableEntityRepository<Points>>();

            var saved = false;

            // To test if the ascent is actually added
            var realAscentsList = new List<Ascent>();

            // To test if points are calculated correctly
            var fakeAscentsList = new List<Ascent>()
            {
                new Ascent()
                {
                    ApplicationUserId = userId,

                    Grade = new Grade()
                    {
                        Points = 100,
                    },

                    Style = new Style()
                    {
                        BonusPoints = 50,
                    },

                    Date = ascentDate,
                },
            };

            var points = new Points()
            {
                ApplicationUserId = userId,
            };

            var pointsList = new List<Points>()
            {
                points,
            };

            var ascentInput = new AscentInputModel()
            {
                BoulderId = boulderId,
                Stars = stars,
                StyleId = styleId,
                Comment = comment,
                GradeId = gradeId,
                Recommend = recommend,
                Date = ascentDate,
            };

            ascentsRepoMock.Setup(x => x.All())
                .Returns(realAscentsList.AsQueryable());

            ascentsRepoMock.Setup(x => x.AllAsNoTracking())
                .Returns(fakeAscentsList.AsQueryable());

            ascentsRepoMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            ascentsRepoMock.Setup(x => x.AddAsync(It.IsAny<Ascent>()))
                .Callback((Ascent ascent) =>
                {
                    realAscentsList.Add(ascent);
                });

            pointsRepoMock.Setup(x => x.All())
                .Returns(pointsList.AsQueryable());

            var ascentsService = new AscentsService(ascentsRepoMock.Object, pointsRepoMock.Object, AutoMapperConfig.MapperInstance);

            // Act
            await ascentsService.AddAsync(ascentInput, userId);

            // Assert
            var ascent = realAscentsList[0];

            Assert.True(saved);
            Assert.Equal(boulderId, ascent.BoulderId);
            Assert.Equal(stars, ascent.Stars);
            Assert.Equal(styleId, ascent.StyleId);
            Assert.Equal(comment, ascent.Comment);
            Assert.Equal(gradeId, ascent.GradeId);
            Assert.Equal(recommend, ascent.Recommend);
            Assert.Equal(ascentDate, ascent.Date);

            Assert.Equal(weeklyPoints, points.Weekly);
            Assert.Equal(monthlyPoints, points.Monthly);
            Assert.Equal(yearlyPoints, points.Yearly);
            Assert.Equal(allTimePoints, points.AllTime);
        }
    }
}
