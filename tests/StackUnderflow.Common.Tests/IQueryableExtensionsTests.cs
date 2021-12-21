using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using StackUnderflow.Common.Base;
using StackUnderflow.Common.Extensions;
using Xunit;

namespace StackUnderflow.Common.Tests
{
    public class IQueryableExtensionsTests
    {
        [Fact]
        public void IQueryableExtensionsTests_CanProjectEntity_Successfully()
        {
            // Arrange
            var query = new List<TestEntity>
            {
                new TestEntity()
            }.AsQueryable();

            // Act
            var result = query.Projector<TestEntity>(null).First();

            // Assert
            Assert.IsType<TestEntity>(result);
        }

        [Fact]
        public void IQueryableExtensionsTests_CanProjectViewModel_Successfully()
        {
            // Arrange
            var query = new List<TestEntity>
            {
                new TestEntity()
            }.AsQueryable();
            var configurationProvider = new MapperConfiguration(
                cfg => cfg.CreateMap<TestEntity, TestEntityDto>());

            // Act
            var result = query.Projector<TestEntityDto>(configurationProvider).First();

            // Assert
            Assert.IsType<TestEntityDto>(result);
        }

        private class TestEntity : BaseEntity<Guid>
        {
        }

        private class TestEntityDto
        {
            public Guid Id { get; set; }
        }
    }
}
