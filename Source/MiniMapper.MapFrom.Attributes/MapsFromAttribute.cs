using System;

namespace MiniMapper.MapFrom.Attributes
{
    /// <summary>
    /// Flags an attribute as mapping from a property on another class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MapsFromAttribute : Attribute
    {
        /// <summary>
        /// The name of the property on the source class
        /// </summary>
        public string SourceName { get; }

        /// <summary>
        /// The source type for the mapping
        /// </summary>
        public Type SourceType { get; set; }

        /// <summary>
        /// Initializes the attribute on the property
        /// </summary>
        /// <param name="sourceName">The name of the property on the source class</param>
        public MapsFromAttribute(string sourceName = null)
        {
            SourceName = sourceName;
        }
    }
}
