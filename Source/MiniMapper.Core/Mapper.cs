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
            if (Maps.Any(x => x.SourceType == typeof (TSource) && x.DestinationType == typeof (TDestination)))
            {
                return;
            }

            var conversions = new List<Conversion>();
            foreach (var property in (typeof (TSource).GetProperties()))
            {
                // Get the mapsTo attribute for the property.  If the attribute does not exist, create one to use with the name of the property
                var mapsToAttribute = property.GetCustomAttribute<MapsToAttribute>() ??
                                      new MapsToAttribute(property.Name);

                if(mapsToAttribute.DestinationType == null || mapsToAttribute.DestinationType == typeof(TDestination))

                {
                    var destinationPropertyName = mapsToAttribute.DestinationName ?? property.Name;
                    var destinationProperty = typeof(TDestination).GetProperty(destinationPropertyName);
                    if ((mapsToAttribute.DestinationType != null) && destinationProperty == null)
                    {
                        throw new DestinationPropertyNotFoundException(typeof(TSource), typeof(TDestination), destinationPropertyName)
                        {
                            Reason = $"Because the property named '{destinationPropertyName}' was not found on the destination type '{typeof(TDestination)}'."
                        };
                    }

                    if(!(mapsToAttribute.DestinationType == null && destinationProperty == null))
                    {
                        var sourceParameter = Expression.Parameter(typeof (TSource));
                        var destinationParameter = Expression.Parameter(typeof (TDestination));
                        var sourcePropertyType = property.PropertyType;
                        var destinationPropertyType = destinationProperty.PropertyType;
                        Expression body;
                        var destinationPropertyExpression = Expression.Property(destinationParameter, destinationPropertyName);
                        if (sourcePropertyType != destinationPropertyType)
                        {
                            var sourcePropertyExpression = Expression.Assign(destinationPropertyExpression,
                                Expression.Convert(Expression.Call(typeof (Convert),
                                    "ChangeType", null, Expression.Property(sourceParameter, property.Name),
                                    Expression.Constant(destinationPropertyType)), destinationPropertyType));
                            body =
                                Expression.Convert(
                                    Expression.Assign(destinationPropertyExpression, sourcePropertyExpression),
                                    typeof(object));
                        }
                        else
                        {
                            var sourcePropertyExpression = Expression.Property(sourceParameter, property.Name);
                            body =
                                Expression.Convert(
                                    Expression.Assign(destinationPropertyExpression, sourcePropertyExpression),
                                    typeof(object));
                        }
                        var expression = Expression.Lambda(body, sourceParameter, destinationParameter);
                        var conversionDelegate = (Func<TSource, TDestination, object>) expression.Compile();

                        if(destinationProperty != null)
                        {
                            conversions.Add(new Conversion
                            {
                                SourceProperty = property.Name,
                                DestinationProperty = destinationPropertyName,
                                Expression = conversionDelegate
                            });
                        }
                    }
                }
            }
            Maps.Add(new Map
            {
                SourceType = typeof(TSource),
                DestinationType = typeof(TDestination),
                Conversions = conversions
            });
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
            var list = new List<TDestination>();
            foreach (var sourceObject in source)
            {
                list.Add(Map<TSource, TDestination>(sourceObject));
            }

            return list;
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
    }
}
