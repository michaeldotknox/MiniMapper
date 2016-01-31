using MiniMapper.MapFrom.Attributes;

namespace MiniMapper.MapFrom.Tests.TestClasses
{
    public class UnmappedDestinationObject
    {
        [MapsFrom("SourceProperty", SourceType = typeof(UnmappedSourceObject))]
        public string UnmappedSourceProperty { get; set; }
    }
}
