using System;
using System.Collections.Generic;
using FluentAssertions;
using MiniMapper.Core;
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
            Func<IEnumerable<Conversion>> action =
                () => _sut.CreateConversions<ComplexSourceObject, DestinationObject>();

            // Assert
            throw new NotImplementedException();
        }

        private void GetSut()
        {
            _sut = new MapsToConversion();
        }
    }
}
