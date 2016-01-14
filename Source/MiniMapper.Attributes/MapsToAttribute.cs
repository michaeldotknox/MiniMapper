using System;

namespace MiniMapper.Attributes
{
    /// <summary>
    /// Flags an attribute as mapping to a property on another class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
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
        public MapsToAttribute(string destinationName = null)
        {
            DestinationName = destinationName;
        }
    }
}
