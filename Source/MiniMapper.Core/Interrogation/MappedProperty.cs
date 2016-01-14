namespace MiniMapper.Core.Interrogation
{
    /// <summary>
    /// This contains the information for the mapped property
    /// </summary>
    public class MappedProperty
    {
        /// <summary>
        /// The name of the property on the source object
        /// </summary>
        public string SourceProperty { get; internal set; }

        /// <summary>
        /// The name of the property on the destination object
        /// </summary>
        public string DestinationProperty { get; internal set; }
    }
}
