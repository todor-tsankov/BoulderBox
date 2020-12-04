using System;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Boulders;
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

            var bouldersService = new AscentsService(ascentsRepoMock.Object, pointsRepoMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await bouldersService.AddAsync(null, "validUserId");
            });
        }

        [Fact]
        public async Task AddAsyncThrowsWhenTheUserIdIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var ascentsRepoMock = new Mock<IDeletableEntityRepository<Ascent>>();
            var pointsRepoMock = new Mock<IDeletableEntityRepository<Points>>();

            var bouldersService = new AscentsService(ascentsRepoMock.Object, pointsRepoMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await bouldersService.AddAsync(new AscentInputModel(), null);
            });
        }
    }
}
