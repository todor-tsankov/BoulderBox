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
using BoulderBox.Web.ViewModels.Forum.Comments;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.ForumServices
{
    public class CommentsServiceTests
    {
        [Fact]
        public void CommentsServiceConstructorThrowsIfCommentsRepositoryIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                new CommentsService(null, mapperMock.Object);
            });
        }

        [Fact]
        public void CommentsServiceConstructorThrowsIfMapperIsNull()
        {
            // Arrange
            var commentsRepoMock = new Mock<IDeletableEntityRepository<Comment>>();

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                new CommentsService(commentsRepoMock.Object, null);
            });
        }

        [Fact]
        public async Task AddAsyncThrowsWhenTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Comment>>();

            var commentsService = new CommentsService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await commentsService.AddAsync(null, "validUserId");
            });
        }

        [Fact]
        public async Task AddAsyncThrowsWhenTheUserIdIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Comment>>();

            var commentsService = new CommentsService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await commentsService.AddAsync(new CommentInputModel(), null);
            });
        }

        [Theory]
        [InlineData("postId1", "text1", "userId1")]
        [InlineData("postId2", "text2", "userId2")]
        [InlineData("postId3", "text3", "userId3")]
        public async Task AddAsyncMapsTheInputModelAndAddsItToTheRepository(string postId, string text, string userId)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly, typeof(ErrorViewModel).Assembly);

            var saved = false;

            var commentInput = new CommentInputModel()
            {
                PostId = postId,
                Text = text,
            };

            var commentsList = new List<Comment>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Comment>>();

            repositoryMock.Setup(x => x.AddAsync(It.IsAny<Comment>()))
                .Callback((Comment comment) =>
                {
                    commentsList.Add(comment);
                });

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var commentsService = new CommentsService(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            // Act
            await commentsService.AddAsync(commentInput, userId);

            // Assert
            var actualComment = commentsList[0];

            Assert.True(saved);
            Assert.Equal(postId, actualComment.PostId);
            Assert.Equal(text, actualComment.Text);
            Assert.Equal(userId, actualComment.ApplicationUserId);
        }

        [Fact]
        public async Task EditAsyncThrowsWhenTheIdIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Comment>>();

            var commentsService = new CommentsService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await commentsService.EditAsync(null, new CommentInputModel());
            });
        }

        [Fact]
        public async Task EditAsyncThrowsWhenTheInputModelIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Comment>>();

            var commentsService = new CommentsService(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await commentsService.EditAsync("validId", null);
            });
        }

        [Theory]
        [InlineData("userId1", "postId1", "text1", "newText1")]
        [InlineData("userId2", "postId2", "text2", "newText2")]
        public async Task EditAsyncEditsThePropertiesAndSavesTheChanges(
            string userId,
            string postId,
            string text,
            string newText)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly, typeof(ErrorViewModel).Assembly);

            var saved = false;
            var comment = new Comment()
            {
                ApplicationUserId = userId,
                Text = text,
                PostId = postId,
            };

            var commentList = new List<Comment>()
            {
                new Comment(),
                comment,
                new Comment(),
                new Comment(),
            };

            var repositoryMock = new Mock<IDeletableEntityRepository<Comment>>();

            repositoryMock.Setup(x => x.All())
                .Returns(commentList.AsQueryable());

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var commentsService = new CommentsService(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            var commentEditModel = new CommentInputModel()
            {
                Text = newText,
                PostId = postId,
            };

            // Act
            await commentsService.EditAsync(comment.Id, commentEditModel);

            // Assert
            Assert.True(saved);

            Assert.Equal(newText, comment.Text);
            Assert.Equal(postId, comment.PostId);
            Assert.Equal(userId, comment.ApplicationUserId);
        }
    }
}
