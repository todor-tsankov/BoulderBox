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
using BoulderBox.Web.ViewModels.Places.Countries;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.PlacesServices
{
    public class CountriesServiceTests
    {
        [Fact]
        public async Task AddAsyncThrowsWhenTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Country>>();

            var countriesService = new CountriesService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await countriesService.AddAsync(null, new ImageInputModel());
            });
        }

        [Theory]
        [InlineData("countryName1", "description1", "AAA", false, "imageId1", "~/img/imageId1.jpeg")]
        [InlineData("countryName2", "",             "BBB", false, "imageId2", "~/img/imageId2.jpeg")]
        [InlineData("countryName3", null,           "CCC", false, "imageId3", "~/img/imageId3.jpeg")]
        [InlineData("countryName4", "description4", "DDD", true, null, null)]
        [InlineData("countryName5", "",             "EEE", true, null, null)]
        [InlineData("countryName6", null,           "FFF", true, null, null)]
        public async Task AddMapsTheInputModelAndImageAndAddsThem(
            string name,
            string description,
            string countryCode,
            bool nullImage,
            string imageId,
            string imageSource)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);

            var saved = true;
            var countriesList = new List<Country>();

            var repositoryMock = new Mock<IDeletableEntityRepository<Country>>();

            repositoryMock.Setup(x => x.AddAsync(It.IsAny<Country>()))
                .Callback((Country country) =>
                {
                    countriesList.Add(country);
                });

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var cityInputModel = new CountryInputModel()
            {
                Name = name,
                CountryCode = countryCode,
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

            var countriesService = new CountriesService(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            // Act
            await countriesService.AddAsync(cityInputModel, imageInputModel);

            // Assert
            var actualCountry = countriesList[0];

            Assert.True(saved);
            Assert.Equal(name, actualCountry.Name);
            Assert.Equal(description, actualCountry.Description);
            Assert.Equal(countryCode, actualCountry.CountryCode);

            var actualImage = actualCountry.Image;

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
