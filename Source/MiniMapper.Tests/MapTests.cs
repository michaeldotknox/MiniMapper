using System;
using FluentAssertions;
using MiniMapper.Core;
using MiniMapper.Tests.TestClasses;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace MiniMapper.Tests
{
    [TestFixture]
    public class MapTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void MapCreatesNewDestinationObject()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void MapThrowsIfMapIsNotCreatedForSourceType()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void MapThrowsIfMapIsNotCreatedForDestinationType()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void SourcePropertyMapsToDestinationPropertyNamed()
        {
            // Arrange
            var source = _fixture.Create<SourceObject>();
            Mapper.CreateMap<SourceObject, DestinationObject>();
            var destination = new DestinationObject();

            // Act
            destination = Mapper.Map(source, destination);

            // Assert
            destination.DestinationProperty.Should().Be(source.SourceProperty);
        }

        [Test]
        public void SourcePropertyMapsToSameDestinationPropertyIfNameIsNotSpecified()
        {
            // Arrange
            var source = _fixture.Create<SourceObject>();
            Mapper.CreateMap<SourceObject, DestinationObject>();
            var destination = new DestinationObject();

            // Act
            destination = Mapper.Map(source, destination);

            // Assert
            destination.SameProperty.Should().Be(source.SameProperty);
        }

        [Test]
        public void SourcePropertyMapsToDestinationPropertyNamedOnlyForSpecifiedType()
        {
            // Arrange
            var source = _fixture.Create<SourceObject>();
            Mapper.CreateMap<SourceObject, DestinationObject>();
            var destination = new DestinationObject();

            // Act
            Mapper.Map(source, destination);

            // Assert
            destination.DestinationMappedProperty.Should().Be(source.SingleMappedProperty);
        }

        [Test]
        public void SourcePropertyDoesNotMapToPropertyOnUnmappedType()
        {
            // Arrange
            var source = _fixture.Create<SourceObject>();
            Mapper.CreateMap<SourceObject, UnmappedDestinationObject>();
            var destination = new UnmappedDestinationObject();

            // Act
            Mapper.Map(source, destination);

            // Assert
            destination.UnmappedProperty.Should().NotBe(source.SingleMappedProperty);
        }
    }
}
