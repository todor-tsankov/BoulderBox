using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

using BoulderBox.Data.Common.Repositories;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Data.Tests.CommonServices.TestClasses;
using BoulderBox.Services.Mapping;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.CommonServices
{
    public class BaseServiceGetManyTests
    {
        [Theory]
        [InlineData("Test2", 2, "Test4", 4, 6, "x => x.Count % 2 == 0")]
        [InlineData("Test1", 1, "Test3", 3, 6, "x => x.Count % 2 != 0")]
        [InlineData("Test2", 2, "Test1", 1, 12, "x => true")]
        [InlineData(null, 1, "", 2, 2, "x => x.Name == null || x.Name == \"\"")]
        public void PredicateFiltersTheDataCorrectly(
            string firstExpectedName,
            int firstExpectedCount,
            string secondExpectedName,
            int secondExpectedCount,
            int expectedCountEntities,
            string predicateStr)
        {
            // Arrange
            var predicate = DynamicExpressionParser.ParseLambda<Test, bool>(new ParsingConfig() { }, true, predicateStr);
            var testData = GetTestData();

            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly);

            var repositoryMock = new Mock<IDeletableEntityRepository<Test>>();
            repositoryMock.Setup(x => x.AllAsNoTracking())
                .Returns(testData);

            var baseService = new BaseService<Test>(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            // Act
            var tests = baseService
                    .GetMany<TestViewModel>(predicate)
                    .ToArray();

            // Assert
            Assert.Equal(firstExpectedName, tests[0].Name);
            Assert.Equal(firstExpectedCount, tests[0].Count);
            Assert.Equal(secondExpectedName, tests[1].Name);
            Assert.Equal(secondExpectedCount, tests[1].Count);
            Assert.Equal(expectedCountEntities, tests.Length);
        }

        [Theory]
        [InlineData(null, 1, "", 2, 12, "x => x.Name", true)]
        [InlineData("Test9", 9, "Test8", 8, 12, "x => x.Name", false)]
        [InlineData("Test10", 10, "Test9", 9, 12, "x => x.Count", false)]
        public void OrderByOrdersByAccordingToAsc(
            string firstExpectedName,
            int firstExpectedCount,
            string secondExpectedName,
            int secondExpectedCount,
            int expectedCountEntities,
            string orderByStr,
            bool asc)
        {
            // Arrange
            var orderBy = DynamicExpressionParser.ParseLambda<Test, object>(new ParsingConfig() { }, true, orderByStr);
            var testData = GetTestData();

            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly);

            var repositoryMock = new Mock<IDeletableEntityRepository<Test>>();
            repositoryMock.Setup(x => x.AllAsNoTracking())
                .Returns(testData);

            var baseService = new BaseService<Test>(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            // Act
            var tests = baseService.GetMany<TestViewModel>(orderBySelector: orderBy, asc: asc).ToList();

            // Assert
            Assert.Equal(firstExpectedName, tests[0].Name);
            Assert.Equal(firstExpectedCount, tests[0].Count);
            Assert.Equal(secondExpectedName, tests[1].Name);
            Assert.Equal(secondExpectedCount, tests[1].Count);
            Assert.Equal(expectedCountEntities, tests.Count);
        }

        [Theory]
        [InlineData("Test2", 2, "Test1", 1, 12, 0)]
        [InlineData("Test1", 1, "Test4", 4, 11, 1)]
        [InlineData("Test4", 4, "Test3", 3, 10, 2)]
        [InlineData("Test3", 3, "Test5", 5, 9, 3)]
        public void SkipSkipsThenecessaryElements(
            string firstExpectedName,
            int firstExpectedCount,
            string secondExpectedName,
            int secondExpectedCount,
            int expectedCountEntities,
            int skip)
        {
            // Arrange
            var testData = GetTestData();

            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly);

            var repositoryMock = new Mock<IDeletableEntityRepository<Test>>();
            repositoryMock.Setup(x => x.AllAsNoTracking())
                .Returns(testData);

            var baseService = new BaseService<Test>(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            // Act
            var tests = baseService.GetMany<TestViewModel>(skip: skip).ToList();

            // Assert
            Assert.Equal(firstExpectedName, tests[0].Name);
            Assert.Equal(firstExpectedCount, tests[0].Count);
            Assert.Equal(secondExpectedName, tests[1].Name);
            Assert.Equal(secondExpectedCount, tests[1].Count);
            Assert.Equal(expectedCountEntities, tests.Count);
        }

        [Theory]
        [InlineData("Test2", 2, "Test1", 1, 2, 2)]
        [InlineData("Test1", 1, "Test4", 4, 3, 3)]
        [InlineData("Test4", 4, "Test3", 3, 4, 4)]
        [InlineData("Test3", 3, "Test5", 5, 5, 5)]
        public void TakeTakesElementsCorrectlyAfterSkip(
            string firstExpectedName,
            int firstExpectedCount,
            string secondExpectedName,
            int secondExpectedCount,
            int expectedCountEntities,
            int take)
        {
            // Arrange
            var testData = GetTestData();

            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly);

            var repositoryMock = new Mock<IDeletableEntityRepository<Test>>();
            repositoryMock.Setup(x => x.AllAsNoTracking())
                .Returns(testData);

            var baseService = new BaseService<Test>(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            // Act
            var tests = baseService.GetMany<TestViewModel>(take: take).ToList();
            var count = tests.Count;

            // Assert
            Assert.Equal(firstExpectedName, tests[count - 2].Name);
            Assert.Equal(firstExpectedCount, tests[count - 2].Count);
            Assert.Equal(secondExpectedName, tests[count - 1].Name);
            Assert.Equal(secondExpectedCount, tests[count - 1].Count);
            Assert.Equal(expectedCountEntities, count);
        }

        [Theory]
        [InlineData("Test1", 1, "Test2", 2, 2, "x => x.Count == 1 || x.Count == 2", "x => x.Name", true, 2, 2)]
        [InlineData("Test6", 6, "Test5", 5, 3, "x => x.Name != null", "x => x.Name", false, 3, 3)]
        [InlineData("Test3", 3, "Test4", 4, 4, "x => x.Name != \"Test5\"", "x => x.Count", true, 4, 4)]
        [InlineData("Test4", 4, "Test3", 3, 5, "x => x.Name != \"Test9\"", "x => x.Count", false, 5, 5)]
        public void CompleteTest(
            string firstExpectedName,
            int firstExpectedCount,
            string secondExpectedName,
            int secondExpectedCount,
            int expectedCountEntities,
            string predicateStr,
            string orderByStr,
            bool asc,
            int skip,
            int take)
        {
            // Arrange
            var predicate = DynamicExpressionParser.ParseLambda<Test, bool>(new ParsingConfig() { }, true, predicateStr);
            var orderBy = DynamicExpressionParser.ParseLambda<Test, object>(new ParsingConfig() { }, true, orderByStr);
            var testData = GetTestData();

            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly);

            var repositoryMock = new Mock<IDeletableEntityRepository<Test>>();
            repositoryMock.Setup(x => x.AllAsNoTracking())
                .Returns(testData);

            var baseService = new BaseService<Test>(repositoryMock.Object, AutoMapperConfig.MapperInstance);

            // Act
            var tests = baseService.GetMany<TestViewModel>(predicate, orderBy, asc, skip, take).ToList();

            // Assert
            Assert.Equal(firstExpectedName, tests[0].Name);
            Assert.Equal(firstExpectedCount, tests[0].Count);
            Assert.Equal(secondExpectedName, tests[1].Name);
            Assert.Equal(secondExpectedCount, tests[1].Count);
            Assert.Equal(expectedCountEntities, tests.Count);
        }

        private static IQueryable<Test> GetTestData()
        {
            var testData = new List<Test>
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
                new Test(null, 1),
                new Test(string.Empty, 2),
            };

            return testData.AsQueryable();
        }
    }
}
