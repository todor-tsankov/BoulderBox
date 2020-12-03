using System.Linq;
using System.Linq.Dynamic.Core;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Data.Tests.CommonServices.TestClasses;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.CommonServices
{
    public class BaseServiceCountTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void ReturnsTheTotalCountWithoutPredicate(int expectedCount)
        {
            // Arrange
            var testData = GetTestData().Take(expectedCount);
            var mapperMock = new Mock<IMapper>();

            var repo = new Mock<IDeletableEntityRepository<Test>>();
            repo.Setup(x => x.AllAsNoTracking())
                .Returns(testData);

            var baseService = new BaseService<Test>(repo.Object, mapperMock.Object);

            // Act
            var actualCount = baseService.Count();

            // Assert
            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData(10, "x => true")]
        [InlineData(5, "x => x.Count % 2 == 0")]
        [InlineData(5, "x => x.Count % 2 != 0")]
        [InlineData(0, "x => false")]
        public void ReturnsTheCorrectCountWithPredicate(int expectedCount, string predicateStr)
        {
            // Arrange
            var predicate = DynamicExpressionParser.ParseLambda<Test, bool>(new ParsingConfig() { }, true, predicateStr);
            var testData = GetTestData();

            var mapperMock = new Mock<IMapper>();

            var repo = new Mock<IDeletableEntityRepository<Test>>();
            repo.Setup(x => x.AllAsNoTracking())
                .Returns(testData);

            var baseService = new BaseService<Test>(repo.Object, mapperMock.Object);

            // Act
            var actualCount = baseService.Count(predicate);

            // Assert
            Assert.Equal(expectedCount, actualCount);
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
