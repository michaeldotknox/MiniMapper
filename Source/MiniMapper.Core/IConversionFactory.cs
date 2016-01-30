using System;
using System.Collections.Generic;
using MiniMapper.Core.Interrogation;

namespace MiniMapper.Core
{
    /// <summary>
    /// Interface that creates a <see cref="Map"/> object.  Use this to create your own maps
    /// </summary>
    public interface IConversionFactory
    {
        IEnumerable<Conversion> CreateConversions<TSource, TDestination>();
    }
}
