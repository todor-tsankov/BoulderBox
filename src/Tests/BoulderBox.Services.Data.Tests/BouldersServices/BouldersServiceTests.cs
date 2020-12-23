using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Boulders;
using BoulderBox.Services.Data.Tests.CommonServices.TestClasses;
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
        public void BouldersServiceConstructorThrowsIfBouldersRepositoryIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                new BouldersService(null, mapperMock.Object);
            });
        }

        [Fact]
        public void BouldersServiceConstructorThrowsIfMapperIsNull()
        {
            // Arrange
            var bouldersRepoMock = new Mock<IDeletableEntityRepository<Boulder>>();

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                new BouldersService(bouldersRepoMock.Object, null);
            });
        }

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
        [InlineData("BoulderName1", "Description1", "sampleGradeId1", "sampleGymId1", "imagesource1", "sampleAuthorId1")]
        [InlineData("BoulderName2", "", "sampleGradeId2", "sampleGymId2", "sampleImageId2", "sampleAuthorId2")]
        [InlineData("BoulderName3", null, "sampleGradeId3", "sampleGymId3", "sampleImageId3", "sampleAuthorId3")]
        public async Task AddAsyncAddsTheEntityToTheRepository(
            string boulderName,
            string boulderDescription,
            string gradeId,
            string gymId,
            string imageSource,
            string authorId)
        {
            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly, typeof(ErrorViewModel).Assembly);

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
            await bouldersService.AddAsync(boulderInput, authorId, imageInput);

            // Assert
            var boulder = testData[0];

            Assert.True(saved);
            Assert.Equal(authorId, boulder.AuthorId);
            Assert.Equal(boulderName, boulder.Name);
            Assert.Equal(boulderDescription, boulder.Description);
            Assert.Equal(gradeId, boulder.GradeId);
            Assert.Equal(gymId, boulder.GymId);
            Assert.Equal(imageInput.Source, boulder.Image.Source);
        }

        [Fact]
        public async Task EditAsyncThrowsWhenIdIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Boulder>>();

            var bouldersService = new BouldersService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await bouldersService.EditAsync(null, new BoulderInputModel(), new ImageInputModel());
            });
        }

        [Fact]
        public async Task EditAsyncThrowsWhenTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Boulder>>();

            var bouldersService = new BouldersService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await bouldersService.EditAsync("validId", null, new ImageInputModel());
            });
        }

        [Theory]
        [InlineData("name1", "desc1", "gradeId1", "gymId1", "newName1", "newDesc1", "newGradeId1", "newGymId1", false, "imageSource1", "newImageSource1")]
        [InlineData("name2", "",      "gradeId2", "gymId2", "newName2", "newDesc2", "newGradeId2", "newGymId2", false, "imageSource2", "newImageSource2")]
        [InlineData("name3", null,    "gradeId3", "gymId3", "newName3", "newDesc3", "newGradeId3", "newGymId3", false, "imageSource3", "newImageSource3")]
        [InlineData("name4", "desc4", "gradeId4", "gymId4", "newName4", "newDesc4", "newGradeId4", "newGymId4", true,  null, null)]
        [InlineData("name5", "",      "gradeId5", "gymId5", "newName5", "newDesc5", "newGradeId5", "newGymId5", true,  null, null)]
        [InlineData("name6", null,    "gradeId6", "gymId6", "newName6", "newDesc6", "newGradeId6", "newGymId6", true,  null, null)]
        public async Task EditAsyncEditsThePorpertiesAndSavesTheChanges(
            string name,
            string description,
            string gradeId,
            string gymId,
            string newName,
            string newDescription,
            string newGradeId,
            string newGymId,
            bool imageNull,
            string imageSource,
            string newImageSource)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly, typeof(ErrorViewModel).Assembly);
            var saved = true;

            var boulder = new Boulder()
            {
                Name = name,
                Description = description,
                GradeId = gradeId,
                GymId = gymId,
                Image = new Image()
                {
                    Source = imageSource,
                },
            };

            var bouldersList = new List<Boulder>()
            {
                new Boulder(),
                new Boulder(),
                new Boulder(),
                boulder,
                new Boulder(),
                new Boulder(),
            };

            var repositoryMock = new Mock<IDeletableEntityRepository<Boulder>>();

            repositoryMock.Setup(x => x.All())
                .Returns(bouldersList.AsQueryable());

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var boulderEditModel = new BoulderInputModel()
            {
                Name = newName,
                Description = newDescription,
                GymId = newGymId,
                GradeId = newGradeId,
            };

            var imageEditModel = new ImageInputModel()
            {
                Source = newImageSource,
            };

            if (imageNull)
            {
                imageEditModel = null;
            }

            var bouldersService = new BouldersService(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            // Act
            await bouldersService.EditAsync(boulder.Id, boulderEditModel, imageEditModel);

            // Assert
            Assert.True(saved);

            Assert.Equal(newName, boulder.Name);
            Assert.Equal(newDescription, boulder.Description);
            Assert.Equal(newGradeId, boulder.GradeId);
            Assert.Equal(newGymId, boulder.GymId);

            var actualImage = boulder.Image;

            if (imageNull)
            {
                Assert.Equal(imageSource, actualImage.Source);
            }
            else
            {
                Assert.Equal(newImageSource, actualImage.Source);
            }
        }
    }
}
