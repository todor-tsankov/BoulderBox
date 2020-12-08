using System;
using System.Collections.Generic;
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
    }
}
