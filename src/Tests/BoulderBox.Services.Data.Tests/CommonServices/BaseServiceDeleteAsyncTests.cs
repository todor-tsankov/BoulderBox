using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Data.Tests.CommonServices.TestClasses;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.CommonServices
{
    public class BaseServiceDeleteAsyncTests
    {
        [Fact]
        public async Task ThrowsWhenTHePreicateIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Test>>();

            var baseService = new BaseService<Test>(repositoryMock.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await baseService.DeleteAsync(null);
            });
        }

        [Theory]
        [InlineData("Test2", "x => true")]
        [InlineData("Test1", "x => x.Name == \"Test1\"")]
        [InlineData("Test4", "x => x.Count == 4")]
        [InlineData("Test10", "x => x.Name == \"Test10\" && x.Count == 10")]
        public async Task FindsAndDeletesTheEntity(string name, string predicateStr)
        {
            // Arrange
            var predicate = DynamicExpressionParser.ParseLambda<Test, bool>(new ParsingConfig() { }, true, predicateStr);
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Test>>();

            var saved = false;
            var testData = GetTestData().ToList();

            repositoryMock.Setup(x => x.All())
                .Returns(testData.AsQueryable());

            repositoryMock.Setup(x => x.Delete(It.IsAny<Test>()))
                .Callback((Test test) =>
                {
                    testData.Remove(test);
                });

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var baseService = new BaseService<Test>(repositoryMock.Object, mapperMock.Object);

            // Act
            var success = await baseService.DeleteAsync(predicate);

            // Assert
            var contains = testData.Any(x => x.Name == name);

            Assert.False(contains);
            Assert.True(success);
            Assert.True(saved);
        }

        [Theory]
        [InlineData("Test1", "x => false")]
        [InlineData("Test2", "x => x.Name == \"Toshko\"")]
        [InlineData("Test3", "x => x.Count < 0")]
        [InlineData("Test4", "x => x.Count == 1 && x.Name == \"Test2\"")]
        public async Task DoesntFindTheEntityAndReturnsFalse(string name, string predicateStr)
        {
            // Arrange
            var predicate = DynamicExpressionParser.ParseLambda<Test, bool>(new ParsingConfig() { }, true, predicateStr);
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Test>>();

            var saved = false;
            var testData = GetTestData();

            repositoryMock.Setup(x => x.AllAsNoTracking())
                .Returns(testData);

            repositoryMock.Setup(x => x.Delete(It.IsAny<Test>()))
                .Callback((Test test) =>
                {
                    testData.ToList().Remove(test);
                });

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() =>
                {
                    saved = true;
                });

            var baseService = new BaseService<Test>(repositoryMock.Object, mapperMock.Object);

            // Act
            var success = await baseService.DeleteAsync(predicate);

            // Assert
            var contains = testData.Any(x => x.Name == name);

            Assert.True(contains);
            Assert.False(success);
            Assert.False(saved);
        }

        private static IQueryable<Test> GetTestData()
        {
            var testData = new Test[]
            {
                new Test("Test2", 2),
                new Test("Test1", 1),
                new Test("Test4", 4),
                new Test("Test3", 3),
                new Test("Test5", 5),
                new Test("Test9", 9),
                new Test("Test7", 7),
                new Test("Test6", 6),
                new Test("Test8", 8),
                new Test("Test10", 10),
            }.AsQueryable<Test>();

            return testData;
        }
    }
}
