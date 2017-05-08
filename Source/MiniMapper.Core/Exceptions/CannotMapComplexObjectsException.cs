using System;

namespace MiniMapper.Core.Exceptions
{
    /// <summary>
    /// Indicates that the current object cannot be mapped because it contains complex objects
    /// </summary>
    public class CannotMapComplexObjectsException : Exception
    {
        /// <summary>
        /// Creates a new exception
        /// </summary>
        public CannotMapComplexObjectsException() : base("Cannot map complex objects with this conversion factory.")
        {
            
        }
    }
}
