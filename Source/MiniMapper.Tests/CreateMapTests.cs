using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MiniMapper.Core;
using MiniMapper.Core.Exceptions;
using MiniMapper.Core.Interrogation;
using MiniMapper.Tests.TestClasses;
using NUnit.Framework;

namespace MiniMapper.Tests
{
    // TODO: With the new interface, some of these tests can be lower level and should be in a different test class
    [TestFixture]
    public class CreateMapTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ClearMapRemovesAllExistingMaps()
        {
            // Arrange
            Mapper.CreateMap<SourceObject, DestinationObject>();

            // Act
            Mapper.ClearMappings();

            // Assert
            var result = Mapper.GetMappings();
            result.Should().BeEmpty();
        }

        [Test]
        public void ClearMapRemovesMappingsOnlyForSpecifiedTypes()
        {
            // Arrange
            Mapper.ClearMappings();
            Mapper.CreateMap<SourceObject, DestinationObject>();
            Mapper.CreateMap<SecondSourceObject, SecondDestinationObject>();

            // Act
            Mapper.ClearMappings<SourceObject, DestinationObject>();

            // Assert
            var result = Mapper.GetMappings();
            result.Should().HaveCount(1);
        }

        [Test]
        public void GetMappingsReturnsListOfMappedObjects()
        {
            // Arrange

            // Act
            var result = Mapper.GetMappings();

            // Assert
            result.Should().BeOfType<List<MappedObject>>();
        }

        [Test]
        public void GetMappingsReturnsListOfMappedObjectsOnlyForSpecifiedObjectTypes()
        {
            // Arrange
            Mapper.ClearMappings();
            Mapper.CreateMap<SourceObject, DestinationObject>();
            Mapper.CreateMap<SecondSourceObject, SecondDestinationObject>();

            // Act
            var result = Mapper.GetMappings<SourceObject, DestinationObject>();

            // Assert
            result.Should().HaveCount(1);
        }

        [Test]
        public void CreateMapCreatesMapWithTheSamePropertyNamesWhenAnAttributeIsNotPresent()
        {
            // Arrange
            Mapper.ClearMappings();

            // Act
            Mapper.CreateMap<SourceObject, DestinationObject>();

            // Assert
            var result = Mapper.GetMappings<SourceObject, DestinationObject>().ToList();
            result.Should().HaveCount(1);
            result[0].Properties.Should().Contain(p => p.SourceProperty == "SameProperty" && p.DestinationProperty == "SameProperty");
        }

        [Test]
        public void CreateMapDoesNotCreateMapIfMapAlreadyExists()
        {
            // Arrange
            Mapper.ClearMappings();

            // Act
            Mapper.CreateMap<SourceObject, DestinationObject>();
            Mapper.CreateMap<SourceObject, DestinationObject>();

            // Assert
            var result = Mapper.GetMappings<SourceObject, DestinationObject>();
            result.Should().HaveCount(1);
        }

        [Test]
        public void CreateMapCreatesAMapWithTheDestinationPropertyNameWhenAnAttributeIsPresent()
        {
            // Arrange
            Mapper.ClearMappings();

            // Act
            Mapper.CreateMap<SourceObject, DestinationObject>();

            // Assert
            var result = Mapper.GetMappings<SourceObject, DestinationObject>().ToList();
            result.Should().HaveCount(1);
            result[0].Properties.Should().Contain(x => x.SourceProperty == "SourceProperty" && x.DestinationProperty == "DestinationProperty");
        }

        [Test]
        public void CreateMapCreatesAMapWithTheDestinationObjectWhenTheObjectTypeIsNotSpecified()
        {
            // Arrange
            Mapper.ClearMappings();

            // Act
            Mapper.CreateMap<SourceObject, DestinationObject>();

            // Assert
            var result = Mapper.GetMappings<SourceObject, DestinationObject>().ToList();
            result.Should().HaveCount(1);
            result[0].Properties.Should().Contain(x => x.SourceProperty == "SingleMappedProperty" && x.DestinationProperty == "DestinationMappedProperty");
        }

        [Test]
        public void CreateMapDoesNotCreateAMapWhenTheDestinationObjectIsSpecifiedButADifferentObjectIsMapped()
        {
            // Arrange
            Mapper.ClearMappings();

            // Act
            Mapper.CreateMap<SourceObject, UnmappedDestinationObject>();

            // Assert
            var result = Mapper.GetMappings<SourceObject, UnmappedDestinationObject>();
            result.First().Properties.Should().BeNull();
        }

        [Test]
        public void
        CreateMapDoesNotThrowExceptionWhenTheDestinationObjectIsNotSpecifiedAndTheDestinationPropertyDoesNotExistOnTheDestinationType()
        {
            // Arrange
            Mapper.ClearMappings();

            // Act
            Action action = Mapper.CreateMap<SourceObject, UnmappedDestinationObject>;

            // Assert
            action.ShouldNotThrow<DestinationPropertyNotFoundException>();
        }

        [Test]
        public void CreateMapThrowsAnExceptionIfTheDestinationTypeIsSpecifiedAndTheDestinationPropertyDoesNotExist()
        {
            // Arrange
            Mapper.ClearMappings();

            // Act
            Action action = Mapper.CreateMap<SourceObjectWithNonExistentDestinationProperty, UnmappedDestinationObject>;

            // Assert
            action.ShouldThrow<DestinationPropertyNotFoundException>();
        }

        [Test]
        public void CreateMapCreatesAMapForTheObjectsAndThePropertiesWithTheSameNameEvenIfAttributesAreNotOnTheProperties()
        {
            // Arrange
            Mapper.ClearMappings();

            // Act
            Mapper.CreateMap<SecondSourceObject, SecondDestinationObject>();

            // Assert
            var result = Mapper.GetMappings<SecondSourceObject, SecondDestinationObject>();
            result.Should().HaveCount(1);
        }

        [Test]
        public void CreateMapCreatesMapForPropertyWithAttributeWhenMatchingPropertiesExist()
        {
            // Arrange
            Mapper.ClearMappings();

            // Act
            Mapper.CreateMap<MultiplePropertySource, MultiplePropertyDestination>();

            // Assert
            var result = Mapper.GetMappings().First();
            result.Properties.Should().OnlyContain(x => x.SourceProperty == "Property1" && x.DestinationProperty == "Property2");
        }
    }
}
