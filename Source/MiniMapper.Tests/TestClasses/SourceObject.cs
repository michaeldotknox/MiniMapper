using MiniMapper.Attributes;

namespace MiniMapper.Tests.TestClasses
{
    public class SourceObject
    {
        [MapsTo("DestinationProperty")]
        public string SourceProperty { get; set; }

        [MapsTo]
        public string SameProperty { get; set; }

        [MapsTo("DestinationMappedProperty", DestinationType = typeof(DestinationObject))]
        public string SingleMappedProperty { get; set; }
    }
}
