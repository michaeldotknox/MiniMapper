using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MiniMapper.Attributes;
using MiniMapper.Core.Exceptions;
using MiniMapper.Core.Interrogation;

namespace MiniMapper.Core
{
    /// <summary>
    /// Overall class for creating maps for objects and mapping objects of one type to another
    /// </summary>
    public static class Mapper
    {
        private static readonly List<Map> Maps; 

        static Mapper()
        {
            Maps = new List<Map>();
        }

        /// <summary>
        /// Creates a map to move the properties from one object to another
        /// </summary>
        /// <typeparam name="TSource">The type of the source object</typeparam>
        /// <typeparam name="TDestination">The type of the destination object</typeparam>
        public static void CreateMap<TSource, TDestination>()
        {
            CreateMap<TSource, TDestination>(new SimpleAttributeConversions());
        }

        /// <summary>
        /// Creates a map to move the properties from one object to another
        /// </summary>
        /// <typeparam name="TSource">The type of the source object</typeparam>
        /// <typeparam name="TDestination">The type of the destination object</typeparam>
        /// <param name="conversionFactory">A <see cref="IConversionFactory"/> object that will create a conversions for the properties on an object</param>
        public static void CreateMap<TSource, TDestination>(IConversionFactory conversionFactory)
        {
            if (Maps.Any(x => x.SourceType == typeof (TSource) && x.DestinationType == typeof (TDestination)))
            {
                return;
            }

            Maps.Add(new Map
            {
                SourceType = typeof (TSource),
                DestinationType = typeof (TDestination),
                Conversions = conversionFactory.CreateConversions<TSource, TDestination>()
            });
        }


        /// <summary>
        /// Creates a map to move the properties from one object to another
        /// </summary>
        /// <typeparam name="TSource">The type of the source object</typeparam>
        /// <typeparam name="TDestination">The type of the destination object</typeparam>
        /// <param name="conversionFactories">An IEnumerable of <see cref="IConversionFactory"/> objects that will create conversions for the properties on an object</param>
        public static void CreateMap<TSource, TDestination>(IEnumerable<IConversionFactory> conversionFactories)
        {
            foreach (var conversionFactory in conversionFactories)
            {
                CreateMap<TSource, TDestination>(conversionFactory);
            }
        }

        /// <summary>
        /// Maps the properties from a source object to the properties on a destination object
        /// </summary>
        /// <typeparam name="TSource">The type of the source object</typeparam>
        /// <typeparam name="TDestination">The type of the destination object</typeparam>
        /// <param name="source">The source object</param>
        /// <param name="destination">The destination object</param>
        /// <returns>The destination object with the fields mapped</returns>
        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (source == null) throw new NullReferenceException();

            var map =
                Maps.FirstOrDefault(x => x.SourceType == typeof (TSource) && x.DestinationType == typeof (TDestination));

            if (map == null)
            {
                throw new MapNotFoundException<TSource, TDestination>();
            }
            var conversions = 
                map.Conversions;

            foreach (var conversion in conversions)
            {
                ((Func<TSource, TDestination, object>)conversion.Expression)
                (source, destination);
            }

            return destination;
        }

        /// <summary>
        /// Maps a list of source objects to a list of destination objects
        /// </summary>
        /// <typeparam name="TSource">The type of the source object</typeparam>
        /// <typeparam name="TDestination">The type of the destination object</typeparam>
        /// <param name="source">An IEnumerable of source objects</param>
        /// <returns>An IEnumerable of destination objects</returns>
        public static IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
            where TDestination : class, new()
        {
            return source.Select(Map<TSource, TDestination>).ToList();
        }

        /// <summary>
        /// Maps the properties from a source object to the properties on a destination object
        /// </summary>
        /// <typeparam name="TSource">The type of the source object</typeparam>
        /// <typeparam name="TDestination">The type of the destination object</typeparam>
        /// <param name="source">The source object</param>
        /// <returns>The new destination object with the fields mapped</returns>
        public static TDestination Map<TSource, TDestination>(TSource source) where TDestination : class, new()
        {
            return source == null ? null : Map(source, new TDestination());
        }

        /// <summary>
        /// Returns a list of mapped objects and their properties
        /// </summary>
        /// <returns>A list of mapped objects</returns>
        public static IEnumerable<MappedObject> GetMappings()
        {
            var result = new List<MappedObject>();

            foreach (var map in Maps)
            {
                var mappedObject = new MappedObject
                {
                    SourceObjectType = map.SourceType.ToString(),
                    DestinationObjectType = map.DestinationType.ToString(),
                };

                var properties = new List<MappedProperty>();

                foreach (var conversion in map.Conversions)
                {
                    properties.Add(new MappedProperty
                    {
                        SourceProperty = conversion.SourceProperty,
                        DestinationProperty = conversion.DestinationProperty
                    });
                    mappedObject.Properties = properties;
                }

                result.Add(mappedObject);
            }

            return result;
        }

        /// <summary>
        /// Returns a list of mapped object and their properties for the specified source and destination types
        /// </summary>
        /// <typeparam name="TSource">The type of the source object</typeparam>
        /// <typeparam name="TDestination">The type of the destination object</typeparam>
        /// <returns>The new destination object with the fields mapped</returns>
        public static IEnumerable<MappedObject> GetMappings<TSource, TDestination>()
        {
            return
                GetMappings()
                    .Where(
                        x =>
                            x.SourceObjectType == typeof (TSource).FullName &&
                            x.DestinationObjectType == typeof (TDestination).FullName);
        } 

        /// <summary>
        /// Removes all of the mappings
        /// </summary>
        public static void ClearMappings()
        {
            Maps.Clear();
        }

        /// <summary>
        /// Removes all of the mappings for a specific source and destination type
        /// </summary>
        /// <typeparam name="TSource">The source type</typeparam>
        /// <typeparam name="TDestination">The destination type</typeparam>
        public static void ClearMappings<TSource, TDestination>()
        {
            Maps.RemoveAll(x => x.SourceType == typeof (TSource) && x.DestinationType == typeof (TDestination));
        }

        private static bool GetNoAttributes<TSource>()
        {
            var properties = typeof(TSource).GetProperties();
            var noAttributes = true;
            foreach (var property in properties.Select(attribute => attribute.GetCustomAttribute<MapsToAttribute>()).Where(property => property != null))
            {
                noAttributes = false;
            }
            return noAttributes;
        }
    }
}
