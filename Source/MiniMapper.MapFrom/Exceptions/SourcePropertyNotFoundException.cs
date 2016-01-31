using System;

namespace MiniMapper.MapFrom.Exceptions
{
    /// <summary>
    /// Exception indicating that the property on the destination object does not exist on the source object
    /// </summary>
    public class SourcePropertyNotFoundException : Exception
    {
        /// <summary>
        /// The source object
        /// </summary>
        public Type SourceType { get; private set; }

        /// <summary>
        /// The destination object
        /// </summary>
        public Type DestinationType { get; private set; }

        /// <summary>
        /// The name of the property that could not be mapped
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// The reason the exception was thrown
        /// </summary>
        public string Reason { get; internal set; }

        internal SourcePropertyNotFoundException(Type sourceType, Type destinationType, string propertyName)
            : base(
                $"Unable to map property '{propertyName}' from source object '{sourceType.FullName}' to destination object '{destinationType.FullName}'."
                )
        {
            SourceType = sourceType;
            DestinationType = destinationType;
            PropertyName = propertyName;
        }
    }
}
