using FluentAssertions;
using MiniMapper.Core;
using MiniMapper.Tests.TestClasses;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace MiniMapper.Tests
{
    [TestFixture]
    public class CreateMapTests
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }
    }
}
