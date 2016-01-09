using System;

namespace MiniMapper.Attributes
{
    /// <summary>
    /// Flags an attribute as mapping to a property on another class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MapsToAttribute : Attribute
    {
        /// <summary>
        /// The name of the property on the destination class
        /// </summary>
        public string DestinationName { get; }

        /// <summary>
        /// The destination type for the mapping
        /// </summary>
        public Type DestinationType { get; set; }

        /// <summary>
        /// Initializes the attribute on the property
        /// </summary>
        /// <param name="destinationName">The name of the property on the destination class</param>
        /// <param name="destinationType">An optional parameter indicating the destination type for mapping</param>
        public MapsToAttribute(string destinationName, Type destinationType = null)
        {
            DestinationName = destinationName;
            DestinationType = destinationType;
        }
    }
}
