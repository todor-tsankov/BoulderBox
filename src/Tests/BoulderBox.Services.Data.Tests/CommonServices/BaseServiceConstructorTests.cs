using System;

using AutoMapper;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Services.Data.Common;
using BoulderBox.Services.Data.Tests.CommonServices.TestClasses;
using Moq;
using Xunit;

namespace BoulderBox.Services.Data.Tests.CommonServices
{
    public class BaseServiceConstructorTests
    {
        [Fact]
        public void ConstructorThrowsWhenBothParametersAreNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                new BaseService<Test>(null, null);
            });
        }

        [Fact]
        public void ConstructorThrowsWhenMapperIsNull()
        {
            // Arrange
            var repositoryMock = new Mock<IDeletableEntityRepository<Test>>();

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                new BaseService<Test>(repositoryMock.Object, null);
            });
        }

        [Fact]
        public void ConstructorThrowsWhenRepoIsNull()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                new BaseService<Test>(null, mapperMock.Object);
            });
        }
    }
}
