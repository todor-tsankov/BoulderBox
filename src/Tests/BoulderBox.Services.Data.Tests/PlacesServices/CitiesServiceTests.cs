using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Places;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Places.Cities;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.PlacesServices
{
    public class CitiesServiceTests
    {
        [Fact]
        public async Task AddAsyncThrowsWhenTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<City>>();

            var citiesService = new CitiesService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await citiesService.AddAsync(null, new ImageInputModel());
            });
        }

        [Theory]
        [InlineData("cityName1", "description1", "countryId1", false, "imageId1", "~/img/imageId1.jpeg")]
        [InlineData("cityName2", "",             "countryId2", false, "imageId2", "~/img/imageId2.jpeg")]
        [InlineData("cityName3", null,           "countryId3", false, "imageId3", "~/img/imageId3.jpeg")]
        [InlineData("cityName1", "description1", "countryId1", true,   null, null)]
        [InlineData("cityName1", "",             "countryId1", true,   null, null)]
        [InlineData("cityName1", null,           "countryId1", true,   null, null)]
        public async Task AddMapsTheInputModelAndImageAndAddsThem(
            string name,
            string description,
            string countryId,
            bool nullImage,
            string imageId,
            string imageSource)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);

            var saved = true;
            var citiesList = new List<City>();

            var repositoryMock = new Mock<IDeletableEntityRepository<City>>();

            repositoryMock.Setup(x => x.AddAsync(It.IsAny<City>()))
                .Callback((City city) =>
                {
                    citiesList.Add(city);
                });

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var cityInputModel = new CityInputModel()
            {
                Name = name,
                Description = description,
                CountryId = countryId,
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

            var citiesService = new CitiesService(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            // Act
            await citiesService.AddAsync(cityInputModel, imageInputModel);

            // Assert
            var actualCity = citiesList[0];

            Assert.True(saved);
            Assert.Equal(name, actualCity.Name);
            Assert.Equal(description, actualCity.Description);
            Assert.Equal(countryId, actualCity.CountryId);

            var actualImage = actualCity.Image;

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
            var repositoryMock = new Mock<IDeletableEntityRepository<City>>();

            var citiesService = new CitiesService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await citiesService.EditAsync(null, new CityInputModel(), new ImageInputModel());
            });
        }

        [Fact]
        public async Task EditAsyncThrowsWhenTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<City>>();

            var citiesService = new CitiesService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await citiesService.EditAsync("validId", null, new ImageInputModel());
            });
        }

        [Theory]
        [InlineData("name1", "desc1", "countryId1", "newName1", "newDesc1", "newCountryId1", false, "imageId1", "imageSource1", "newImageId1", "newImageSource1")]
        [InlineData("name2", "",      "countryId2", "newName2", "newDesc2", "newCountryId2", false, "imageId2", "imageSource2", "newImageId2", "newImageSource2")]
        [InlineData("name3", null,    "countryId3", "newName3", "newDesc3", "newCountryId3", false, "imageId3", "imageSource3", "newImageId3", "newImageSource3")]
        [InlineData("name4", "desc4", "countryId4", "newName4", "newDesc4", "newCountryId4", true, null, null, null, null)]
        [InlineData("name5", "",      "countryId5", "newName5", "newDesc5", "newCountryId5", true, null, null, null, null)]
        [InlineData("name6", null,    "countryId6", "newName6", "newDesc6", "newCountryId6", true, null, null, null, null)]
        public async Task EdidAsyncSetsTheNewPropertiesAndSavesTheResult(
            string name,
            string description,
            string countryId,
            string newName,
            string newDescription,
            string newCountryId,
            bool imageNull,
            string imageId,
            string imageSource,
            string newImageId,
            string newImageSource)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);
            var saved = true;

            var city = new City()
            {
                Name = name,
                Description = description,
                CountryId = countryId,
                Image = new Image()
                {
                    Id = imageId,
                    Source = imageSource
                },
            };

            var citiesList = new List<City>()
            {
                new City(),
                city,
                new City(),
                new City(),
                new City(),
            };

            var repositoryMock = new Mock<IDeletableEntityRepository<City>>();

            repositoryMock.Setup(x => x.All())
                .Returns(citiesList.AsQueryable());

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var citiesService = new CitiesService(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            var cityEditModel = new CityInputModel()
            {
                Name = newName,
                Description = newDescription,
                CountryId = newCountryId,
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

            // Act
            await citiesService.EditAsync(city.Id, cityEditModel, imageEditModel);

            // Assert
            Assert.True(saved);

            Assert.Equal(newName, city.Name);
            Assert.Equal(newDescription, city.Description);
            Assert.Equal(newCountryId, city.CountryId);

            var actualImage = city.Image;

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
