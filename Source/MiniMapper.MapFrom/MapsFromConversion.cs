using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MiniMapper.Core;
using MiniMapper.Core.Interrogation;
using MiniMapper.MapFrom.Attributes;
using MiniMapper.MapFrom.Exceptions;

namespace MiniMapper.MapFrom
{
    /// <summary>
    /// Conversion to map using the <see cref="MapsFromAttribute"/> to map specifying the source property.
    /// Useful when you do not have the ability to decorate the source object
    /// </summary>
    public class MapsFromConversion : IConversionFactory
    {
        /// <summary>
        /// Creates the conversions for mapping properties from the source object to the destination object
        /// </summary>
        /// <typeparam name="TSource">The source object type</typeparam>
        /// <typeparam name="TDestination">The destination object type</typeparam>
        /// <returns>An IEnumerable of <see cref="Conversion"/>s</returns>
        public IEnumerable<Conversion> CreateConversions<TSource, TDestination>()
        {
            var conversions = new List<Conversion>();

            var properties = (typeof(TDestination).GetProperties()).Where(x => x.GetCustomAttribute<MapsFromAttribute>() != null).ToArray();
            foreach (var property in properties)
            {
                // Get the mapsTo attribute for the property.  If the attribute does not exist, create one to use with the name of the property
                var mapsFromAttribute = property.GetCustomAttribute<MapsFromAttribute>() ??
                                      new MapsFromAttribute(property.Name);

                if (mapsFromAttribute.SourceType == null || mapsFromAttribute.SourceType == typeof(TSource))

                {
                    var sourcePropertyName = mapsFromAttribute.SourceName;
                    var sourceProperty = typeof(TSource).GetProperty(sourcePropertyName);
                    if ((mapsFromAttribute.SourceType != null) && sourceProperty == null)
                    {
                        throw new SourcePropertyNotFoundException(typeof(TSource), typeof(TDestination), sourcePropertyName)
                        {
                            Reason = $"Because the property named '{sourcePropertyName}' was not found on the source type '{typeof(TSource)}'."
                        };
                    }

                    if (!(mapsFromAttribute.SourceType == null && sourceProperty == null))
                    {
                        var sourceParameter = Expression.Parameter(typeof(TSource));
                        var destinationParameter = Expression.Parameter(typeof(TDestination));
                        var sourcePropertyType = sourceProperty.PropertyType;
                        var destinationPropertyType = property.PropertyType;
                        Expression body;
                        var sourcePropertyExpression = Expression.Property(sourceParameter, sourcePropertyName);
                        if (sourcePropertyType != destinationPropertyType)
                        {
                            if (destinationPropertyType == typeof(string))
                            {
                                var destinationPropertyExpression = Expression.Assign(sourcePropertyExpression,
                                    Expression.Call(Expression.Property(sourceParameter, property.Name), "ToString",
                                        null));
                                body =
                                    Expression.Convert(
                                        Expression.Assign(destinationPropertyExpression, sourcePropertyExpression),
                                        typeof(object));
                            }
                            else
                            {
                                var destinationPropertyExpression = Expression.Assign(sourcePropertyExpression,
                                    Expression.Convert(Expression.Call(typeof(Convert),
                                        "ChangeType", null, Expression.Property(sourceParameter, property.Name),
                                        Expression.Constant(destinationPropertyType)), destinationPropertyType));
                                body =
                                    Expression.Convert(
                                        Expression.Assign(destinationPropertyExpression, sourcePropertyExpression),
                                        typeof(object));
                            }
                        }
                        else
                        {
                            var destinationPropertyExpression = Expression.Property(destinationParameter, property.Name);
                            body =
                                Expression.Convert(
                                    Expression.Assign(destinationPropertyExpression, sourcePropertyExpression),
                                    typeof(object));
                        }
                        var expression = Expression.Lambda(body, sourceParameter, destinationParameter);
                        var conversionDelegate = (Func<TSource, TDestination, object>)expression.Compile();

                        conversions.Add(new Conversion
                        {
                            SourceProperty = sourcePropertyName,
                            DestinationProperty = property.Name,
                            Expression = conversionDelegate
                        });
                    }
                }
            }
            return conversions;
        }
    }
}
