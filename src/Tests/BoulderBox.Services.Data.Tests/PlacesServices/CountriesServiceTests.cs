using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Places;
using BoulderBox.Services.Data.Tests.CommonServices.TestClasses;
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
        [InlineData("countryName1", "description1", "AAA", false, "/img/imageId1.jpeg")]
        [InlineData("countryName2", "",             "BBB", false, "/img/imageId2.jpeg")]
        [InlineData("countryName3", null,           "CCC", false, "/img/imageId3.jpeg")]
        [InlineData("countryName4", "description4", "DDD", true, null)]
        [InlineData("countryName5", "",             "EEE", true, null)]
        [InlineData("countryName6", null,           "FFF", true, null)]
        public async Task AddMapsTheInputModelAndImageAndAddsThem(
            string name,
            string description,
            string countryCode,
            bool nullImage,
            string imageSource)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly, typeof(ErrorViewModel).Assembly);

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
                Assert.Equal(imageSource, actualImage.Source);
            }
        }

        [Fact]
        public async Task EditAsyncThrowsWhenTheIdIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Country>>();

            var countriesService = new CountriesService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await countriesService.EditAsync(null, new CountryInputModel(), new ImageInputModel());
            });
        }

        [Fact]
        public async Task EditAsyncThrowsWhenTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Country>>();

            var countriesService = new CountriesService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await countriesService.EditAsync("validId", null, new ImageInputModel());
            });
        }

        [Theory]
        [InlineData("countryName1", "desc1", "AAA", "editedName1", "editedDesc1", "CCC", false, "imageSrc1", "editedImageSrc1")]
        [InlineData("countryName2", "",      "BBB", "editedName2", "editedDesc2", "AAA", false, "imageSrc2", "editedImageSrc2")]
        [InlineData("countryName4", null,    "CCC", "editedName2", "editedDesc2", "DDD", false, "imageSrc3", "editedImageSrc3")]
        [InlineData("countryName4", "desc4", "DDD", "editedName3", "editedDesc3", "BBB", true, "imageSrc4", null)]
        [InlineData("countryName5", "desc5", "EEE", "editedName4", "",            "FFF", true, "imageSrc5", null)]
        [InlineData("countryName6", "desc6", "FFF", "editedName4", null,          "EEE", true, "imageSrc6", null)]
        public async Task EditAsyncEditsTheCorrectPropertiesAndSavesTheResult(
            string countryName,
            string description,
            string countryCode,
            string newCountryName,
            string newDescription,
            string newCountryCode,
            bool imageNull,
            string imageSource,
            string newImageSource)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly, typeof(ErrorViewModel).Assembly);
            var saved = true;

            var country = new Country()
            {
                Name = countryName,
                CountryCode = countryCode,
                Description = description,
                Image = new Image()
                {
                    Source = imageSource,
                },
            };

            var countriesList = new List<Country>()
            {
                new Country(),
                new Country(),
                country,
                new Country(),
            };

            var repositoryMock = new Mock<IDeletableEntityRepository<Country>>();

            repositoryMock.Setup(x => x.All())
                .Returns(countriesList.AsQueryable());

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var countriesService = new CountriesService(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            var editInputModel = new CountryInputModel()
            {
                Name = newCountryName,
                CountryCode = newCountryCode,
                Description = newDescription,
            };

            var imageInputModel = new ImageInputModel()
            {
                Source = newImageSource,
            };

            if (imageNull)
            {
                imageInputModel = null;
            }

            // Act
            await countriesService.EditAsync(country.Id, editInputModel, imageInputModel);

            // Assert
            Assert.True(saved);

            Assert.Equal(newCountryName, country.Name);
            Assert.Equal(newCountryCode, country.CountryCode);
            Assert.Equal(newDescription, country.Description);

            var actualImage = country.Image;

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
