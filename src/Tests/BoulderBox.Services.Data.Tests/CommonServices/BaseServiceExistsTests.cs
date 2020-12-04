using System;
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
    public class BaseServiceExistsTests
    {
        [Fact]
        public void ThrowsIfThePredicateIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();

            var repo = new Mock<IDeletableEntityRepository<Test>>();
            var baseSerivce = new BaseService<Test>(repo.Object, mapperMock.Object);

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                baseSerivce.Exists(null);
            });
        }

        [Theory]
        [InlineData(true, "x => true")]
        [InlineData(true, "x => x.Name == \"Test5\"")]
        [InlineData(true, "x => x.Count == 5")]
        [InlineData(false, "x => false")]
        [InlineData(false, "x => x.Name == \"Invalid\"")]
        [InlineData(false, "x => x.Count == 100")]
        public void ReturnsWhetherSuchEntityExists(bool expected, string predicateStr)
        {
            // Arrange
            var predicate = DynamicExpressionParser.ParseLambda<Test, bool>(new ParsingConfig() { }, true, predicateStr);
            var mapperMock = new Mock<IMapper>();

            var repositoryMock = new Mock<IDeletableEntityRepository<Test>>();
            repositoryMock.Setup(x => x.AllAsNoTracking())
                .Returns(GetTestData());

            var baseService = new BaseService<Test>(repositoryMock.Object, mapperMock.Object);

            // Act
            var actual = baseService.Exists(predicate);

            // Assert
            Assert.Equal(expected, actual);
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
