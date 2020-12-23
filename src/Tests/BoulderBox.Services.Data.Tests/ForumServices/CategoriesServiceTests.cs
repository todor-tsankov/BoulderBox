using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Forum;
using BoulderBox.Services.Data.Tests.CommonServices.TestClasses;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Forum.Categories;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.ForumServices
{
    public class CategoriesServiceTests
    {
        [Fact]
        public void CategoriesServiceConstructorThrowsIfCategoriesRepositoryIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                new CategoriesService(null, mapperMock.Object);
            });
        }

        [Fact]
        public void CategoriesServiceConstructorThrowsIfMapperIsNull()
        {
            // Arrange
            var categoriesRepoMock = new Mock<IDeletableEntityRepository<Category>>();

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                new CategoriesService(categoriesRepoMock.Object, null);
            });
        }

        [Fact]
        public async Task AddAsyncThrowsIfTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Category>>();

            var categoriesService = new CategoriesService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await categoriesService.AddAsync(null, new ImageInputModel());
            });
        }

        [Theory]
        [InlineData("name1", "desc1", false, "/img/imageId1.jpg")]
        [InlineData("name2", "",      false, "/img/imageId2.jpg")]
        [InlineData("name3", null,    false, "/img/imageId3.jpg")]
        [InlineData("name4", "desc4", true, null)]
        [InlineData("name5", "",      true, null)]
        [InlineData("name6", null,    true, null)]
        public async Task AddAsyncMapsTheInputModelAndAddsItToTheRepository(
            string name,
            string description,
            bool nullImage,
            string imageSource)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly, typeof(ErrorViewModel).Assembly);

            var saved = false;
            var categoriesList = new List<Category>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Category>>();

            repositoryMock.Setup(x => x.AddAsync(It.IsAny<Category>()))
                .Callback((Category category) =>
                {
                    categoriesList.Add(category);
                });

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var categoriesService = new CategoriesService(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            var categoryInput = new CategoryInputModel()
            {
                Name = name,
                Description = description,
            };

            var imageInput = new ImageInputModel()
            {
                Source = imageSource,
            };

            if (nullImage)
            {
                imageInput = null;
            }

            // Act
            await categoriesService.AddAsync(categoryInput, imageInput);

            // Assert
            var actualCategory = categoriesList[0];

            Assert.True(saved);
            Assert.Equal(name, actualCategory.Name);
            Assert.Equal(description, actualCategory.Description);

            var actualImage = actualCategory.Image;

            if (nullImage)
            {
                Assert.Null(actualCategory.Image);
            }
            else
            {
                Assert.Equal(imageSource, actualImage.Source);
            }
        }

        [Fact]
        public async Task EditAsyncThrowsIfTheIdIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Category>>();

            var categoriesService = new CategoriesService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await categoriesService.EditAsync(null, new CategoryInputModel(), new ImageInputModel());
            });
        }

        [Fact]
        public async Task EditAsyncThrowsIfTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Category>>();

            var categoriesService = new CategoriesService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await categoriesService.EditAsync("validId", null, new ImageInputModel());
            });
        }

        [Theory]
        [InlineData("name1", "desc1", "newName1", "newDesc1", false, "imageSource1", "newImageSource1")]
        [InlineData("name2", "",      "newName2", "newDesc2", false, "imageSource2", "newImageSource2")]
        [InlineData("name3", null,    "newName3", "newDesc3", false, "imageSource3", "newImageSource3")]
        [InlineData("name4", "desc4", "newName4", "newDesc1", true, null, null)]
        [InlineData("name5", "desc5", "newName5", "",         true, null, null)]
        [InlineData("name6", "desc6", "newName6", null,       true, null, null)]
        public async Task EditAsyncEditsThePropertiesAndSavesTheChanges(
            string name,
            string description,
            string newName,
            string newDescription,
            bool imageNull,
            string imageSource,
            string newImageSource)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly, typeof(ErrorViewModel).Assembly);

            var saved = false;

            var category = new Category()
            {
                Name = name,
                Description = description,
                Image = new Image()
                {
                    Source = imageSource,
                },
            };

            var categoriesList = new List<Category>()
            {
                new Category(),
                new Category(),
                category,
                new Category(),
                new Category(),
            };

            var repositoryMock = new Mock<IDeletableEntityRepository<Category>>();

            repositoryMock.Setup(x => x.All())
                .Returns(categoriesList.AsQueryable());

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var categoriesService = new CategoriesService(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            var categoryEditModel = new CategoryInputModel()
            {
                Name = newName,
                Description = newDescription,
            };

            var imageEditModel = new ImageInputModel()
            {
                Source = newImageSource,
            };

            if (imageNull)
            {
                imageEditModel = null;
            }

            // Act
            await categoriesService.EditAsync(category.Id, categoryEditModel, imageEditModel);

            // Assert
            Assert.True(saved);

            Assert.Equal(newName, category.Name);
            Assert.Equal(newDescription, category.Description);

            var actualImage = category.Image;

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
