using System;
using System.Collections.Generic;

namespace MiniMapper.Core
{
    internal class Map
    {
        internal Type SourceType { get; set; }
        internal Type DestinationType { get; set; }
        internal IEnumerable<object> Conversions { get; set; }
    }
}
