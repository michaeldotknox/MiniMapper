using System;
using System.Linq;
using FluentAssertions;
using MiniMapper.Core;
using MiniMapper.Core.Exceptions;
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
            Mapper.ClearMappings();
            Mapper.CreateMap<SourceObject, DestinationObject>();
            var source = _fixture.Create<SourceObject>();

            // Act
            var destination = Mapper.Map<SourceObject, DestinationObject>(source);

            // Assert
            destination.Should().NotBeNull();
        }

        [Test]
        public void MapThrowsMapNotFoundExceptionIfTheMappingDoesNotExistForTheObjectTypes()
        {
            // Arrange
            Mapper.ClearMappings();
            var source = _fixture.Create<SourceObject>();

            // Act
            Action action = () => Mapper.Map<SourceObject, DestinationObject>(source);

            // Assert
            action.ShouldThrow<MapNotFoundException<SourceObject, DestinationObject>>();
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

        [Test]
        public void ListOfSourceObjectsMapsToListOfDestinationObjects()
        {
            // Arrange
            var sources = _fixture.CreateMany<SourceObject>().ToList();
            Mapper.ClearMappings();
            Mapper.CreateMap<SourceObject, DestinationObject>();

            // Act
            var results = Mapper.Map<SourceObject, DestinationObject>(sources).ToList();

            // Assert
            results.Should().HaveSameCount(sources);
            for (var i = 0; i < sources.Count; i++)
            {
                sources[i].SourceProperty.Should().Be(results[i].DestinationProperty);
                sources[i].SameProperty.Should().Be(results[i].SameProperty);
            }
        }

        [Test]
        public void ANullObjectShouldMapToNull()
        {
            // Arrange
            Mapper.ClearMappings();
            Mapper.CreateMap<SourceObject, DestinationObject>();
            SourceObject source = null;

            // Act
            var result = Mapper.Map<SourceObject, DestinationObject>(source);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void MapThrowsNullReferenceExceptionIfTheSourceObjectIsNullAndTheDestinationObjectIsProvided()
        {
            // Arrange
            Mapper.ClearMappings();
            Mapper.CreateMap<SourceObject, DestinationObject>();
            SourceObject source = null;
            var destination = new DestinationObject();

            // Act
            Action action = () => Mapper.Map<SourceObject, DestinationObject>(source);

            // Assert
            action.ShouldNotThrow<NullReferenceException>();
        }

        [Test]
        public void MapConvertsStringPropertyToIntegerProperty()
        {
            // Arrange
            Mapper.ClearMappings();
            Mapper.CreateMap<StringObject, IntegerObject>();
            var source = new StringObject {Integer = "5"};

            // Act
            var destination = Mapper.Map<StringObject, IntegerObject>(source);

            // Assert
            destination.Integer.Should().Be(int.Parse(source.Integer));
        }

        [Test]
        public void MapConvertsStringPropertyToLongProperty()
        {
            // Arrange
            Mapper.ClearMappings();
            Mapper.CreateMap<StringObject, LongObject>();
            var source = new StringObject {Long = "5"};

            // Act
            var destination = Mapper.Map<StringObject, LongObject>(source);
            
            // Assert
            destination.Long.Should().Be(long.Parse(source.Long));
        }

        [Test]
        public void MapConvertsStringPropertyToDoubleProperty()
        {
            // Arrange
            Mapper.ClearMappings();
            Mapper.CreateMap<StringObject, DoubleObject>();
            var source = new StringObject {Double = "5.5"};

            // Act
            var destination = Mapper.Map<StringObject, DoubleObject>(source);

            // Assert
            destination.Double.Should().Be(Double.Parse(source.Double));
        }

        [Test]
        public void MapConvertsStringPropertyToDecimalProperty()
        {
            // Arrange
            Mapper.ClearMappings();
            Mapper.CreateMap<StringObject, DecimalObject>();
            var source = new StringObject {Decimal = "5.5"};

            // Act
            var destination = Mapper.Map<StringObject, DecimalObject>(source);

            // Assert
            destination.Decimal.Should().Be(decimal.Parse(source.Decimal));
        }

        [Test]
        public void MapConvertsStringPropertyToDateTimeProperty()
        {
            // Arrange
            Mapper.ClearMappings();
            Mapper.CreateMap<StringObject, DateTimeObject>();
            var source = new StringObject {DateTime = "1/1/2016"};

            // Act
            var destination = Mapper.Map<StringObject, DateTimeObject>(source);

            // Assert
            destination.DateTime.Should().Be(DateTime.Parse(source.DateTime));
        }

        [Test]
        public void MapConvertsStringPropertyToBooleanPropertyAsTrue()
        {
            // Arrange
            Mapper.ClearMappings();
            Mapper.CreateMap<StringObject, BooleanObject>();
            var source = new StringObject { Boolean = "true" };

            // Act
            var destination = Mapper.Map<StringObject, BooleanObject>(source);

            // Assert
            destination.Boolean.Should().Be(bool.Parse(source.Boolean));
        }
    }
}
