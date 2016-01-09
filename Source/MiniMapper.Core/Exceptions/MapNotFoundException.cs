using System;

namespace MiniMapper.Core.Exceptions
{
    /// <summary>
    /// An exception when the requested source object type has not been registered
    /// </summary>
    /// <typeparam name="TSource">The type of the source object</typeparam>
    /// <typeparam name="TDestination">The type of the destination object</typeparam>
    public class MapNotFoundException<TSource, TDestination> : Exception
    {
        internal MapNotFoundException() : base("A map was not found for a source of" + typeof(TSource) + " with a destination of " + typeof(TDestination))
        {
            
        }
    }
}
