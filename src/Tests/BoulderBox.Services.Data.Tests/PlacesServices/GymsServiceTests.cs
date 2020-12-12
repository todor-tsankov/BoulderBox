using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Places;
using BoulderBox.Services.Data.Tests.CommonServices.TestClasses;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Places.Gyms;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.PlacesServices
{
    public class GymsServiceTests
    {
        [Fact]
        public async Task AddAsyncThrowsWhenTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Gym>>();

            var gymsService = new GymsService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await gymsService.AddAsync(null, new ImageInputModel());
            });
        }

        [Theory]
        [InlineData("gymName1", "description1", "cityId1", false, "imageId1", "~/img/imageId1.jpeg")]
        [InlineData("gymName2", "", "cityId2", false, "imageId2", "~/img/imageId2.jpeg")]
        [InlineData("gymName3", null, "cityId3", false, "imageId3", "~/img/imageId3.jpeg")]
        [InlineData("gymName4", "description4", "cityId4", true, null, null)]
        [InlineData("gymName5", "", "cityId5", true, null, null)]
        [InlineData("gymName6", null, "cityId6", true, null, null)]
        public async Task AddMapsTheInputModelAndImageAndAddsThem(
            string name,
            string description,
            string cityId,
            bool nullImage,
            string imageId,
            string imageSource)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly, typeof(ErrorViewModel).Assembly);

            var saved = true;
            var gymsList = new List<Gym>();

            var repositoryMock = new Mock<IDeletableEntityRepository<Gym>>();

            repositoryMock.Setup(x => x.AddAsync(It.IsAny<Gym>()))
                .Callback((Gym gym) =>
                {
                    gymsList.Add(gym);
                });

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var gymViewModel = new GymInputModel()
            {
                Name = name,
                CityId = cityId,
                Description = description,
            };

            var imageInputModel = new ImageInputModel()
            {
                Id = imageId,
                Source = imageSource,
            };

            if (nullImage)
            {
                imageInputModel = null;
            }

            var gymsService = new GymsService(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            // Act
            await gymsService.AddAsync(gymViewModel, imageInputModel);

            // Assert
            var actualGym = gymsList[0];

            Assert.True(saved);
            Assert.Equal(name, actualGym.Name);
            Assert.Equal(cityId, actualGym.CityId);
            Assert.Equal(description, actualGym.Description);

            var actualImage = actualGym.Image;

            if (nullImage)
            {
                Assert.Null(actualImage);
            }
            else
            {
                Assert.Equal(imageId, actualImage.Id);
                Assert.Equal(imageSource, actualImage.Source);
            }
        }

        [Fact]
        public async Task EditAsyncThrowsWhenTheIdIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Gym>>();

            var gymsService = new GymsService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await gymsService.EditAsync(null, new GymInputModel(), new ImageInputModel());
            });
        }

        [Fact]
        public async Task EditAsyncThrowsWhenTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Gym>>();

            var gymsService = new GymsService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await gymsService.EditAsync("validId", null, new ImageInputModel());
            });
        }

        [Theory]
        [InlineData("name1", "desc1", "cityId1", "newName1", "newDesc1", "newCityId1", false, "imageId1", "imageSerc1", "newImageId1", "newImageSrc1")]
        [InlineData("name2", "",      "cityId2", "newName2", "newDesc2", "newCityId2", false, "imageId2", "imageSerc2", "newImageId1", "newImageSrc1")]
        [InlineData("name3", null,    "cityId3", "newName3", "newDesc3", "newCityId3", false, "imageId3", "imageSerc3", "newImageId1", "newImageSrc1")]
        [InlineData("name4", "desc4", "cityId4", "newName4", "newDesc4", "newCityId4", false, null, null, null, null)]
        [InlineData("name5", "",      "cityId5", "newName5", "newDesc5", "newCityId5", false, null, null, null, null)]
        [InlineData("name6", null,    "cityId6", "newName6", "newDesc6", "newCityId6", false, null, null, null, null)]
        public async Task EditAsyncEditsThePropertiesAndSavesTheChanges(
            string name,
            string description,
            string cityId,
            string newName,
            string newDescription,
            string newCityId,
            bool imageNull,
            string imageId,
            string imageSource,
            string newImageId,
            string newImageSource)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly, typeof(ErrorViewModel).Assembly);
            var saved = true;

            var gym = new Gym()
            {
                Name = name,
                Description = description,
                CityId = cityId,
            };

            var gymsList = new List<Gym>()
            {
                new Gym(),
                gym,
                new Gym(),
                new Gym(),
            };

            var repositoryMock = new Mock<IDeletableEntityRepository<Gym>>();

            repositoryMock.Setup(x => x.All())
                .Returns(gymsList.AsQueryable());

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var gymEditModel = new GymInputModel()
            {
                Name = newName,
                Description = newDescription,
                CityId = newCityId,
            };

            var imageEditModel = new ImageInputModel()
            {
                Id = newImageId,
                Source = newImageSource,
            };

            if (imageNull)
            {
                imageEditModel = null;
            }

            var gymsService = new GymsService(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            // Act
            await gymsService.EditAsync(gym.Id, gymEditModel, imageEditModel);

            // Assert
            Assert.True(saved);

            Assert.Equal(newName, gym.Name);
            Assert.Equal(newDescription, gym.Description);
            Assert.Equal(newCityId, gym.CityId);

            var actualImage = gym.Image;

            if (imageNull)
            {
                Assert.Equal(imageId, actualImage.Id);
                Assert.Equal(imageSource, actualImage.Source);
            }
            else
            {
                Assert.Equal(newImageId, actualImage.Id);
                Assert.Equal(newImageSource, actualImage.Source);
            }
        }
    }
}
