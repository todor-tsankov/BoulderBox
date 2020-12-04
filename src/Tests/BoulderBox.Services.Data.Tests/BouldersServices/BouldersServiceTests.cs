using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Boulders;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels;
using BoulderBox.Web.ViewModels.Boulders.Boulders;
using BoulderBox.Web.ViewModels.Files.Images;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.BouldersServices
{
    public class BouldersServiceTests
    {
        [Fact]
        public async Task AddAsyncThrowsWhenTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Boulder>>();

            var bouldersService = new BouldersService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await bouldersService.AddAsync(null, "validId", new ImageInputModel());
            });
        }

        [Fact]
        public async Task AddAsyncThrowsWhenTheAuthorIdIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Boulder>>();

            var bouldersService = new BouldersService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await bouldersService.AddAsync(new BoulderInputModel(), null, new ImageInputModel());
            });
        }

        [Fact]
        public async Task AddAsyncThrowsWhenTheImageInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Boulder>>();

            var bouldersService = new BouldersService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await bouldersService.AddAsync(new BoulderInputModel(), "validId", null);
            });
        }

        [Theory]
        [InlineData("BoulderName1", "Description1", "sampleGradeId1", "sampleGymId1", "sampleImageId1", "imagesource1", "sampleAuthorId1")]
        [InlineData("BoulderName2", "", "sampleGradeId2", "sampleGymId2", "sampleImageId2", "imagesource2", "sampleAuthorId2")]
        [InlineData("BoulderName3", null, "sampleGradeId3", "sampleGymId3", "sampleImageId3", "imagesource3", "sampleAuthorId3")]
        public async Task AddAsyncAddsTheEntityToTheRepository(
            string boulderName,
            string boulderDescription,
            string gradeId,
            string gymId,
            string imageId,
            string imageSource,
            string authorId)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);

            var repositoryMock = new Mock<IDeletableEntityRepository<Boulder>>();
            var testData = new List<Boulder>();

            var boulderInput = new BoulderInputModel()
            {
                Name = boulderName,
                Description = boulderDescription,
                GradeId = gradeId,
                GymId = gymId,
            };

            var imageInput = new ImageInputModel()
            {
                Id = imageId,
                Source = imageSource,
            };

            var saved = false;

            repositoryMock.Setup(x => x.AddAsync(It.IsAny<Boulder>()))
                .Callback((Boulder boulder) =>
                {
                    testData.Add(boulder);
                });

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var bouldersService = new BouldersService(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            // Act
            var success = await bouldersService.AddAsync(boulderInput, authorId, imageInput);

            // Assert
            var boulder = testData[0];

            Assert.True(saved);
            Assert.True(success);
            Assert.Equal(authorId, boulder.AuthorId);
            Assert.Equal(boulderName, boulder.Name);
            Assert.Equal(boulderDescription, boulder.Description);
            Assert.Equal(gradeId, boulder.GradeId);
            Assert.Equal(gymId, boulder.GymId);
            Assert.Equal(imageInput.Id, boulder.Image.Id);
            Assert.Equal(imageInput.Source, boulder.Image.Source);
        }
    }
}
