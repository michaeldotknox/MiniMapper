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
        /// <summary>
        /// Creates the conversion expressions to map the properties of one object to the properties of another object
        /// </summary>
        /// <typeparam name="TSource">The source object type</typeparam>
        /// <typeparam name="TDestination">The destination object type</typeparam>
        /// <returns>A IEnumerable of <see cref="Conversion"/></returns>
        IEnumerable<Conversion> CreateConversions<TSource, TDestination>();
    }
}
