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
        [InlineData("categoryId1", "text1", "title1", false, "imageSource1", "userId1")]
        [InlineData("categoryId2", "text2", "title2", false, "imageSource2", "userId2")]
        [InlineData("categoryId3", "text3", "title3", true, null, "userId3")]
        [InlineData("categoryId4", "text4", "title4", true, null, "userId4")]
        public async Task AddMapsTheInputModelsSetsUserIdAndAddsToTheRepository(
            string categoryId,
            string text,
            string title,
            bool nullImage,
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
                Assert.Equal(imageSource, actualImage.Source);
            }
        }

        [Fact]
        public async Task EditAsyncThrowsWhenTheIdIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Post>>();

            var postsService = new PostsService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await postsService.EditAsync(null, new PostInputModel(), new ImageInputModel());
            });
        }

        [Fact]
        public async Task EditAsyncThrowsWhenTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Post>>();

            var postsService = new PostsService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await postsService.EditAsync("validId", null, new ImageInputModel());
            });
        }

        [Theory]
        [InlineData("userId1", "title1", "text1", "catId1", "newTitle1", "newText1", "newCatId1", false, "imageId1", "newImageId1")]
        [InlineData("userId2", "title2", "text2", "catId2", "newTitle2", "newText2", "newCatId2", false, "imageId1", "newImageId1")]
        [InlineData("userId3", "title3", "text3", "catId3", "newTitle3", "newText3", "newCatId3", true, null, null)]
        [InlineData("userId4", "title4", "text4", "catId4", "newTitle4", "newText4", "newCatId4", true, null, null)]
        public async Task EditAsyncEditsThePropertiesAndSavesTheChanges(
            string userId,
            string title,
            string text,
            string categoryId,
            string newTitle,
            string newText,
            string newCategoryId,
            bool imageNull,
            string imageSource,
            string newImageSource)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly, typeof(ErrorViewModel).Assembly);
            var saved = false;

            var post = new Post()
            {
                ApplicationUserId = userId,
                Title = title,
                Text = text,
                CategoryId = categoryId,
                Image = new Image()
                {
                    Source = imageSource,
                }
            };

            var postsList = new List<Post>()
            {
                new Post(),
                post,
                new Post(),
                new Post(),
                new Post(),
            };

            var repositoryMock = new Mock<IDeletableEntityRepository<Post>>();

            repositoryMock.Setup(x => x.All())
                .Returns(postsList.AsQueryable());

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var postsService = new PostsService(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            var postEditModel = new PostInputModel()
            {
                Title = newTitle,
                Text = newText,
                CategoryId = newCategoryId,
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
            await postsService.EditAsync(post.Id, postEditModel, imageEditModel);

            // Assert
            Assert.True(saved);
            Assert.Equal(userId, post.ApplicationUserId);

            Assert.Equal(newTitle, post.Title);
            Assert.Equal(newText, post.Text);
            Assert.Equal(newCategoryId, post.CategoryId);

            var actualImage = post.Image;

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
