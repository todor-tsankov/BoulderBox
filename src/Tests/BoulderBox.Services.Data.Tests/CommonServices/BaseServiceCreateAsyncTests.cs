using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Data.Tests.CommonServices.TestClasses;
using BoulderBox.Services.Mapping;
using BoulderBox.Web.ViewModels;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.CommonServices
{
    public class BaseServiceCreateAsyncTests
    {
        [Fact]
        public async Task ThrowsIfTheInputObjectIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();

            var repo = new Mock<IDeletableEntityRepository<Test>>();
            var baseSerivce = new BaseService<Test>(repo.Object, mapperMock.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await baseSerivce.AddAsync(null);
            });
        }

        [Theory]
        [InlineData("", 4)]
        [InlineData(null, 0)]
        [InlineData("NewTest1", 1)]
        [InlineData("NewTest2", 2)]
        [InlineData("NewTest3", 3)]
        public async Task MapsTheEntityAndAddsIt(string name, int count)
        {
            // Arrange
            AutoMapperConfig.RegisterMappings(typeof(Test).Assembly, typeof(ErrorViewModel).Assembly);
            var repositoryMock = new Mock<IDeletableEntityRepository<Test>>();

            var testData = GetTestData();
            var saved = false;

            repositoryMock.Setup(x => x.AddAsync(It.IsAny<Test>()))
                .Callback((Test test) =>
                {
                    testData.Add(test);
                });

            repositoryMock.Setup(x => x.SaveChangesAsync())
                .Callback(() => { saved = true; });

            var baseSerivce = new BaseService<Test>(repositoryMock.Object, AutoMapperConfig.MapperInstance);
            var testViewModel = new TestViewModel(name, count);

            // Act
            await baseSerivce.AddAsync(testViewModel);

            // Assert
            var exists = testData.Any(x => x.Name == name && x.Count == count);

            Assert.True(exists);
            Assert.True(saved);
        }

        private static List<Test> GetTestData()
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
            };

            return testData;
        }
    }
}
