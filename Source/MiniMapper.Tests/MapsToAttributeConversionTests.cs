using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MiniMapper.Core;
using MiniMapper.Core.Exceptions;
using MiniMapper.Core.Interrogation;
using MiniMapper.Tests.TestClasses;
using NUnit.Framework;

namespace MiniMapper.Tests
{
    [TestFixture]
    public class MapsToAttributeConversionTests
    {
        private MapsToConversion _sut;

        [SetUp]
        public void Setup()
        {
            GetSut();
        }

        [Test]
        public void CreateConversionsThrowsIfPropertyTypesAreComplex()
        {
            // Arrange

            // Act
            Func<Task<IEnumerable<Conversion>>> action =
                () => Task.Run(() => _sut.CreateConversions<ComplexSourceObject, DestinationObject>());

            // Assert
            action.ShouldThrow<CannotMapComplexObjectsException>();
        }

        private void GetSut()
        {
            _sut = new MapsToConversion();
        }
    }
}
