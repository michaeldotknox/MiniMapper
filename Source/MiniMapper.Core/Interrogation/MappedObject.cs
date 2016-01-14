using System.Collections.Generic;

namespace MiniMapper.Core.Interrogation
{
    /// <summary>
    /// Contains the information for the mapped objects
    /// </summary>
    public class MappedObject
    {
        /// <summary>
        /// The type of the source object
        /// </summary>
        public string SourceObjectType { get; internal set; }

        /// <summary>
        /// The type of the destination object
        /// </summary>
        public string DestinationObjectType { get; internal set; }

        /// <summary>
        /// The list of properties mapped
        /// </summary>
        public IEnumerable<MappedProperty> Properties { get; internal set; }
    }
}
