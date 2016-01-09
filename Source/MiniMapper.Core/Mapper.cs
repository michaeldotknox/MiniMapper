﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MiniMapper.Attributes;

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
            var conversions = new List<object>();
            foreach (
                var property in
                    (typeof (TSource).GetProperties().Where(x => x.GetCustomAttribute(typeof (MapsToAttribute)) != null))
                )
            {
                var attribute = property.GetCustomAttribute<MapsToAttribute>();
                if(attribute.DestinationType == null || attribute.DestinationType == typeof(TDestination))
                {
                    var sourceParameter = Expression.Parameter(typeof (TSource));
                    var destinationParameter = Expression.Parameter(typeof (TDestination));
                    var body = Expression.Convert(Expression.Assign(
                        Expression.PropertyOrField(destinationParameter, attribute.DestinationName),
                        Expression.PropertyOrField(sourceParameter, property.Name)), typeof(object));
                    var expression = Expression.Lambda(body, sourceParameter, destinationParameter);
                    var conversionDelegate = (Func<TSource, TDestination, object>) expression.Compile();

                conversions.Add(conversionDelegate);
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
            var map =
                Maps.FirstOrDefault(x => x.SourceType == typeof (TSource) && x.DestinationType == typeof (TDestination));

            if (map == null)
            {
                throw new Exception();
            }
            var conversions = 
                map.Conversions;

            foreach (var conversionDelegate in conversions)
            {
                ((Func<TSource, TDestination, object>)conversionDelegate)
                (source, destination);
            }

            return destination;
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
            return Map(source, new TDestination());
        }
    }
}
