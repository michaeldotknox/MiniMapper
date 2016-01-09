using MiniMapper.Attributes;

namespace TestConsole
{
    public class DataModel
    {
        [MapsTo("StringProperty")]
        public string StringField { get; set; }
        [MapsTo("IntegerProperty")]
        public int IntegerField { get; set; }
    }
}
