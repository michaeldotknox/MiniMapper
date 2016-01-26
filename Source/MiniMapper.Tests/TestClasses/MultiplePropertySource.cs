using MiniMapper.Attributes;

namespace MiniMapper.Tests.TestClasses
{
    public class MultiplePropertySource
    {
        [MapsTo("Property2")]
        public string Property1 { get; set; }
        public string Property2 { get; set; }
    }
}
