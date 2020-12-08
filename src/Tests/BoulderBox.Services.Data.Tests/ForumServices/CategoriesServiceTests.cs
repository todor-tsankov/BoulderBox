﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Forum;
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
        [InlineData("name1", "desc1", false, "imageId1", "~/img/imageId1.jpg")]
        [InlineData("name2", "",      false, "imageId2", "~/img/imageId2.jpg")]
        [InlineData("name3", null,    false, "imageId3", "~/img/imageId3.jpg")]
        [InlineData("name4", "desc4", true,  null, null)]
        [InlineData("name5", "",      true,  null, null)]
        [InlineData("name6", null,    true,  null, null)]
        public async Task AddAsyncMapsTheInputModelAndAddsItToTheRepository(
            string name,
            string description,
            bool nullImage,
            string imageId,
            string imageSource)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);

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
                Id = imageId,
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

            if (nullImage)
            {
                Assert.Null(actualCategory.Image);
            }
            else
            {
                var actualImage = actualCategory.Image;

                Assert.Equal(imageId, actualImage.Id);
                Assert.Equal(imageSource, actualImage.Source);
            }
        }
    }
}