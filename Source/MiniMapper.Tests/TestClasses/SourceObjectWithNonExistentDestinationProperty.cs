using MiniMapper.Attributes;

namespace MiniMapper.Tests.TestClasses
{
    public class SourceObjectWithNonExistentDestinationProperty
    {
        [MapsTo(DestinationType = typeof(UnmappedDestinationObject))]
        public string NonExistentProperty { get; set; }
    }
}
