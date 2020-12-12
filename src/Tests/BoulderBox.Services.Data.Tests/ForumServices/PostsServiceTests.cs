using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Forum;
using BoulderBox.Services.Data.Tests.CommonServices.TestClasses;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels;
using BoulderBox.Web.ViewModels.Files.Images;
using BoulderBox.Web.ViewModels.Forum.Posts;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.ForumServices
{
    public class PostsServiceTests
    {
        [Fact]
        public async Task AddAsyncThrowsWhenTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Post>>();

            var postsService = new PostsService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await postsService.AddAsync(null, new ImageInputModel(), "validUserId");
            });
        }

        [Fact]
        public async Task AddAsyncThrowsWhenTheUserIdIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Post>>();

            var postsService = new PostsService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await postsService.AddAsync(new PostInputModel(), new ImageInputModel(), null);
            });
        }

        [Theory]
        [InlineData("categoryId1", "text1", "title1", false, "imageId1", "imageSource1", "userId1")]
        [InlineData("categoryId2", "text2", "title2", false, "imageId2", "imageSource2", "userId2")]
        [InlineData("categoryId3", "text3", "title3", true, null, null, "userId3")]
        [InlineData("categoryId4", "text4", "title4", true, null, null, "userId4")]
        public async Task AddMapsTheInputModelsSetsUserIdAndAddsToTheRepository(
            string categoryId,
            string text,
            string title,
            bool nullImage,
            string imageId,
            string imageSource,
            string userId)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly, typeof(ErrorViewModel).Assembly);

            var saved = false;

            var postsList = new List<Post>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Post>>();

            repositoryMock.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) =>
                {
                    postsList.Add(post);
                });

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var postsService = new PostsService(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            var postInputModel = new PostInputModel()
            {
                CategoryId = categoryId,
                Text = text,
                Title = title,
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

            // Act
            await postsService.AddAsync(postInputModel, imageInputModel, userId);

            // Assert
            var actualPost = postsList[0];

            Assert.True(saved);
            Assert.Equal(categoryId, actualPost.CategoryId);
            Assert.Equal(text, actualPost.Text);
            Assert.Equal(title, actualPost.Title);
            Assert.Equal(userId, actualPost.ApplicationUserId);

            var actualImage = actualPost.Image;

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
