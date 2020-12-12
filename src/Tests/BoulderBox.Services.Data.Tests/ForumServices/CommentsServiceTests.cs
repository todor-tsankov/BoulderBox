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
using BoulderBox.Web.ViewModels.Forum.Comments;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.ForumServices
{
    public class CommentsServiceTests
    {
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
    }
}
