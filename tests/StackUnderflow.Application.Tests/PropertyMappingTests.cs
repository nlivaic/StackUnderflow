using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using StackUnderflow.Application.Sorting;
using StackUnderflow.Application.Tests.Helpers;
using Xunit;

namespace StackUnderflow.Application.Tests
{
    public class PropertyMappingTests
    {
        [Fact]
        public void PropertyMappingService_OneSourceAndTargetModelWithOneSimpleMapping_MapsSuccessfully()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty1)}")
                }
            };
            var target = new PropertyMappingService(options);

            // Act
            var targetMapping = target.GetMapping<MappingSourceModel1, MappingTargetModel1>(nameof(MappingSourceModel1.SourceProperty1));

            // Assert
            Assert.Single(targetMapping.TargetPropertyNames);
            Assert.Equal($"{nameof(MappingSourceModel1.SourceProperty1)}", targetMapping.SourcePropertyName);
            Assert.Equal($"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty1)}", targetMapping.TargetPropertyNames.First());
            Assert.False(targetMapping.Revert);
        }

        [Fact]
        public void PropertyMappingService_OneSourceAndTargetModelWithOneSimpleMappingReverted_MapsSuccessfully()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(true, nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty1)}")
                }
            };
            var target = new PropertyMappingService(options);

            // Act
            var targetMapping = target.GetMapping<MappingSourceModel1, MappingTargetModel1>(nameof(MappingSourceModel1.SourceProperty1));

            // Assert
            Assert.Single(targetMapping.TargetPropertyNames);
            Assert.Equal($"{nameof(MappingSourceModel1.SourceProperty1)}", targetMapping.SourcePropertyName);
            Assert.Equal($"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty1)}", targetMapping.TargetPropertyNames.First());
            Assert.True(targetMapping.Revert);
        }

        [Fact]
        public void PropertyMappingService_OneSourceAndTargetModelWithSeveralSimpleMapping_MapsSuccessfully()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty1)}")
                        .Add(nameof(MappingSourceModel1.SourceProperty2), $"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty2)}")
                }
            };
            var target = new PropertyMappingService(options);

            // Act
            var targetMapping1 = target.GetMapping<MappingSourceModel1, MappingTargetModel1>(nameof(MappingSourceModel1.SourceProperty1));
            var targetMapping2 = target.GetMapping<MappingSourceModel1, MappingTargetModel1>(nameof(MappingSourceModel1.SourceProperty2));

            // Assert - first mapping
            Assert.Single(targetMapping1.TargetPropertyNames);
            Assert.Equal($"{nameof(MappingSourceModel1.SourceProperty1)}", targetMapping1.SourcePropertyName);
            Assert.Equal($"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty1)}", targetMapping1.TargetPropertyNames.First());
            Assert.False(targetMapping1.Revert);
            // Assert - second mapping
            Assert.Single(targetMapping2.TargetPropertyNames);
            Assert.Equal($"{nameof(MappingSourceModel1.SourceProperty2)}", targetMapping2.SourcePropertyName);
            Assert.Equal($"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty2)}", targetMapping2.TargetPropertyNames.First());
            Assert.False(targetMapping2.Revert);
        }

        [Fact]
        public void PropertyMappingService_OneSourceAndTargetModelWithSeveralComplexMappings_MapsSuccessfully()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty1)}", $"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty2)}")
                        .Add(nameof(MappingSourceModel1.SourceProperty2), $"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty3)}", $"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty4)}")
                }
            };
            var target = new PropertyMappingService(options);

            // Act
            var targetMapping1 = target.GetMapping<MappingSourceModel1, MappingTargetModel1>(nameof(MappingSourceModel1.SourceProperty1));
            var targetMapping2 = target.GetMapping<MappingSourceModel1, MappingTargetModel1>(nameof(MappingSourceModel1.SourceProperty2));

            // Assert - first mapping
            Assert.Equal(2, targetMapping1.TargetPropertyNames.Count());
            Assert.Equal($"{nameof(MappingSourceModel1.SourceProperty1)}", targetMapping1.SourcePropertyName);
            Assert.Equal($"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty1)}", targetMapping1.TargetPropertyNames.ToList()[0]);
            Assert.Equal($"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty2)}", targetMapping1.TargetPropertyNames.ToList()[1]);
            Assert.False(targetMapping1.Revert);
            // Assert - second mapping
            Assert.Equal(2, targetMapping2.TargetPropertyNames.Count());
            Assert.Equal($"{nameof(MappingSourceModel1.SourceProperty2)}", targetMapping2.SourcePropertyName);
            Assert.Equal($"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty3)}", targetMapping2.TargetPropertyNames.ToList()[0]);
            Assert.Equal($"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty4)}", targetMapping2.TargetPropertyNames.ToList()[1]);
            Assert.False(targetMapping2.Revert);
        }

        [Fact]
        public void PropertyMappingService_TwoSourceAndTargetModelsWithSeveralComplexMappings_MapsSuccessfully()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty1)}", $"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty2)}")
                        .Add(true, nameof(MappingSourceModel1.SourceProperty2), $"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty3)}", $"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty4)}"),
                    new PropertyMapping<MappingSourceModel2, MappingTargetModel2>()
                        .Add(nameof(MappingSourceModel2.SourceProperty1), $"{nameof(MappingTargetModel2)}.{nameof(MappingTargetModel2.TargetProperty1)}", $"{nameof(MappingTargetModel2)}.{nameof(MappingTargetModel2.TargetProperty2)}")
                        .Add(true, nameof(MappingSourceModel2.SourceProperty2), $"{nameof(MappingTargetModel2)}.{nameof(MappingTargetModel2.TargetProperty3)}", $"{nameof(MappingTargetModel2)}.{nameof(MappingTargetModel2.TargetProperty4)}")
                }
            };
            var target = new PropertyMappingService(options);

            // Act - first pair
            var targetMapping1 = target.GetMapping<MappingSourceModel1, MappingTargetModel1>(nameof(MappingSourceModel1.SourceProperty1));
            var targetMapping2 = target.GetMapping<MappingSourceModel1, MappingTargetModel1>(nameof(MappingSourceModel1.SourceProperty2));
            // Act - second pair
            var targetMapping3 = target.GetMapping<MappingSourceModel2, MappingTargetModel2>(nameof(MappingSourceModel2.SourceProperty1));
            var targetMapping4 = target.GetMapping<MappingSourceModel2, MappingTargetModel2>(nameof(MappingSourceModel2.SourceProperty2));

            // Assert - first pair, first mapping
            Assert.Equal(2, targetMapping1.TargetPropertyNames.Count());
            Assert.Equal($"{nameof(MappingSourceModel1.SourceProperty1)}", targetMapping1.SourcePropertyName);
            Assert.Equal($"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty1)}", targetMapping1.TargetPropertyNames.ToList()[0]);
            Assert.Equal($"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty2)}", targetMapping1.TargetPropertyNames.ToList()[1]);
            Assert.False(targetMapping1.Revert);
            // Assert - second pair, second mapping
            Assert.Equal(2, targetMapping2.TargetPropertyNames.Count());
            Assert.Equal($"{nameof(MappingSourceModel1.SourceProperty2)}", targetMapping2.SourcePropertyName);
            Assert.Equal($"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty3)}", targetMapping2.TargetPropertyNames.ToList()[0]);
            Assert.Equal($"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty4)}", targetMapping2.TargetPropertyNames.ToList()[1]);
            Assert.True(targetMapping2.Revert);
            // Assert - second pair, first mapping
            Assert.Equal(2, targetMapping3.TargetPropertyNames.Count());
            Assert.Equal($"{nameof(MappingSourceModel2.SourceProperty1)}", targetMapping3.SourcePropertyName);
            Assert.Equal($"{nameof(MappingTargetModel2)}.{nameof(MappingTargetModel2.TargetProperty1)}", targetMapping3.TargetPropertyNames.ToList()[0]);
            Assert.Equal($"{nameof(MappingTargetModel2)}.{nameof(MappingTargetModel2.TargetProperty2)}", targetMapping3.TargetPropertyNames.ToList()[1]);
            Assert.False(targetMapping3.Revert);
            // Assert - second pair, second mapping
            Assert.Equal(2, targetMapping4.TargetPropertyNames.Count());
            Assert.Equal($"{nameof(MappingSourceModel2.SourceProperty2)}", targetMapping4.SourcePropertyName);
            Assert.Equal($"{nameof(MappingTargetModel2)}.{nameof(MappingTargetModel2.TargetProperty3)}", targetMapping4.TargetPropertyNames.ToList()[0]);
            Assert.Equal($"{nameof(MappingTargetModel2)}.{nameof(MappingTargetModel2.TargetProperty4)}", targetMapping4.TargetPropertyNames.ToList()[1]);
            Assert.True(targetMapping4.Revert);
        }

        [Fact]
        public void PropertyMappingService_UnknownPropertyMappingWhenGettingSingleProperty_Throws()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty1)}")
                }
            };
            var target = new PropertyMappingService(options);

            // Act, Assert
            Assert.Throws<InvalidPropertyMappingException>(() => target.GetMapping<MappingSourceModel1, MappingTargetModel1>(nameof(MappingSourceModel1.SourceProperty2)));
        }

        [Fact]
        public void PropertyMappingService_UnknownSourceAndTargetTypeMappingWhenGettingSingleProperty_Throws()
        {
            // Arrange
            var options = new PropertyMappingOptions
            {
                PropertyMappings = new List<IPropertyMapping>
                {
                    new PropertyMapping<MappingSourceModel1, MappingTargetModel1>()
                        .Add(nameof(MappingSourceModel1.SourceProperty1), $"{nameof(MappingTargetModel1)}.{nameof(MappingTargetModel1.TargetProperty1)}")
                }
            };
            var target = new PropertyMappingService(options);

            // Act, Assert
            Assert.Throws<InvalidPropertyMappingException>(() => target.GetMapping<MappingSourceModel1, MappingTargetModel2>(nameof(MappingSourceModel1.SourceProperty1)));
        }
    }
}
