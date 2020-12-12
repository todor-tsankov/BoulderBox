using System;
using System.Linq;
using System.Linq.Dynamic.Core;

using AutoMapper;
using AutoMapper.Configuration;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Data.Tests.CommonServices.TestClasses;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.CommonServices
{
    public class BaseServiceGetSingleTests
    {
        [Fact]
        public void ThrowsIfPredicateIsNull()
        {
            // Arrange
            var mock = new Mock<IDeletableEntityRepository<Test>>();
            var mapperMock = new Mock<IMapper>();

            var baseService = new BaseService<Test>(mock.Object, mapperMock.Object);

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                baseService.GetSingle<TestViewModel>(null);
            });
        }

        [Theory]
        [InlineData("", 10, "x => x.Count == 10")]
        [InlineData("Test1", 1, "x => true")]
        [InlineData("Test2", 2, "x => x.Name == \"Test2\"")]
        [InlineData("Test3", 3, "x => x.Name == \"Test3\" && x.Count == 3")]
        public void ReturnsTheCorrectPropertyMapped(string expectedName, int expectedCount, string predicateStr)
        {
            // Arrange
            var predicate = DynamicExpressionParser.ParseLambda<Test, bool>(new ParsingConfig() { }, true, predicateStr);
            var testData = GetTestData();

            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly, typeof(ErrorViewModel).Assembly);

            var repository = new Mock<IDeletableEntityRepository<Test>>();
            repository.Setup(x => x.AllAsNoTracking())
                .Returns(testData);

            var baseSerice = new BaseService<Test>(repository.Object, AutoMapperConfig.MapperInstance);

            // Act
            var test = baseSerice.GetSingle<TestViewModel>(predicate);

            // Assert
            Assert.Equal(expectedName, test.Name);
            Assert.Equal(expectedCount, test.Count);
        }

        [Theory]
        [InlineData("x => false")]
        [InlineData("x => x.Count < 0")]
        [InlineData("x => x.Name == \"Toshko\"")]
        public void DoesntFindTheEnitityAndReturnsNull(string predicateStr)
        {
            // Arrange
            var predicate = DynamicExpressionParser.ParseLambda<Test, bool>(new ParsingConfig() { }, true, predicateStr);
            var mapperMock = new Mock<IMapper>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Test>>();

            repositoryMock.Setup(x => x.AllAsNoTracking())
                .Returns(GetTestData());

            var baseService = new BaseService<Test>(repositoryMock.Object, mapperMock.Object);

            // Act
            var test = baseService.GetSingle<TestViewModel>(predicate);

            // Assert
            Assert.Null(test);
        }

        private static IQueryable<Test> GetTestData()
        {
            var testData = new Test[]
            {
                new Test("Test1", 1),
                new Test("Test2", 2),
                new Test("Test1", 1),
                new Test("Test4", 4),
                new Test("Test3", 3),
                new Test("Test5", 5),
                new Test("Test9", 9),
                new Test("Test7", 7),
                new Test("Test6", 6),
                new Test("Test8", 8),
                new Test(string.Empty, 10),
            }.AsQueryable<Test>();

            return testData;
        }
    }
}
