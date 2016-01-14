using System;
using System.Collections.Generic;
using MiniMapper.Core.Interrogation;

namespace MiniMapper.Core
{
    internal class Map
    {
        internal Type SourceType { get; set; }
        internal Type DestinationType { get; set; }
        internal IEnumerable<Conversion> Conversions { get; set; }
    }
}
