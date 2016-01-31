using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MiniMapper.Core.Interrogation;
using MiniMapper.MapFrom.Exceptions;
using MiniMapper.MapFrom.Tests.TestClasses;
using NUnit.Framework;

namespace MiniMapper.MapFrom.Tests
{
    [TestFixture]
    public class CreateMapTests
    {
        private MapsFromConversion _sut;

        [SetUp]
        public void Setup()
        {
            GetSut();
        }

        [Test]
        public void CreateMapCreatesAMapWithTheDestinationPropertyNameWhenAnAttributeIsPresent()
        {
            // Arrange

            // Act
            var result = _sut.CreateConversions<SourceObject, DestinationObject>().ToList();

            // Assert
            result.Should()
                .Contain(x => x.SourceProperty == "SourceProperty" && x.DestinationProperty == "DestinationProperty");
        }

        [Test]
        public void CreateMapCreatesAMapWithTheDestinationObjectWhenTheObjectTypeIsNotSpecified()
        {
            // Arrange

            // Act
            var result = _sut.CreateConversions<SourceObject, DestinationObject>().ToList();

            // Assert
            result.Should()
                .Contain(x => x.SourceProperty == "SourceProperty" && x.DestinationProperty == "DestinationProperty");
        }

        [Test]
        public void CreateMapDoesNotCreateAMapWhenTheDestinationObjectIsSpecifiedButADifferentObjectIsMapped()
        {
            // Arrange

            // Act
            var result = _sut.CreateConversions<UnmappedSourceObject, DestinationObject>();

            // Assert
            result.Should().HaveCount(0);
        }

        [Test]
        public void
        CreateMapDoesNotThrowExceptionWhenTheSourceObjectIsNotSpecifiedAndTheSourcePropertyDoesNotExistOnTheSourceType()
        {
            // Arrange

            // Act
            Func<IEnumerable<Conversion>> action = () => _sut.CreateConversions<UnmappedSourceObject, DestinationObject>();

            // Assert
            action.Enumerating().ShouldNotThrow();
        }

        [Test]
        public void CreateMapThrowsAnExceptionIfTheSourceTypeIsSpecifiedAndTheSourcePropertyDoesNotExist()
        {
            // Arrange

            // Act
            Func<IEnumerable<Conversion>> action =
                () => _sut.CreateConversions<UnmappedSourceObject, UnmappedDestinationObject>();

            // Assert
            action.Enumerating().ShouldThrow<SourcePropertyNotFoundException>();
        }

        private void GetSut()
        {
            _sut = new MapsFromConversion();
        }
    }
}
