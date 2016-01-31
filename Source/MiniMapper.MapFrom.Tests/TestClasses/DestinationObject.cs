using MiniMapper.MapFrom.Attributes;

namespace MiniMapper.MapFrom.Tests.TestClasses
{
    public class DestinationObject
    {
        [MapsFrom("SourceProperty")]
        public string DestinationProperty { get; set; }

        [MapsFrom("SourceProperty", SourceType = typeof(SourceObject))]
        public string UnmappedProperty { get; set; }
    }
}
