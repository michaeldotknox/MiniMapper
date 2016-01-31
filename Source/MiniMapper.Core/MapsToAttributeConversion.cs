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
    internal class MapsToAttributeConversion : IConversionFactory
    {
        public IEnumerable<Conversion> CreateConversions<TSource, TDestination>()
        {
            var noAttributes = GetNoAttributes<TSource>();

            var conversions = new List<Conversion>();
            PropertyInfo[] properties;
            if (noAttributes)
            {
                properties = (typeof(TSource).GetProperties());
            }
            else
            {
                properties = (typeof(TSource).GetProperties()).Where(x => x.GetCustomAttribute<MapsToAttribute>() != null).ToArray();
            }
            foreach (var property in properties)
            {
                // Get the mapsTo attribute for the property.  If the attribute does not exist, create one to use with the name of the property
                var mapsToAttribute = property.GetCustomAttribute<MapsToAttribute>() ??
                                      new MapsToAttribute(property.Name);

                if (mapsToAttribute.DestinationType == null || mapsToAttribute.DestinationType == typeof(TDestination))

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

                    if (!(mapsToAttribute.DestinationType == null && destinationProperty == null))
                    {
                        var sourceParameter = Expression.Parameter(typeof(TSource));
                        var destinationParameter = Expression.Parameter(typeof(TDestination));
                        var sourcePropertyType = property.PropertyType;
                        var destinationPropertyType = destinationProperty.PropertyType;
                        Expression body;
                        var destinationPropertyExpression = Expression.Property(destinationParameter, destinationPropertyName);
                        if (sourcePropertyType != destinationPropertyType)
                        {
                            if (destinationPropertyType == typeof(string))
                            {
                                var sourcePropertyExpression = Expression.Assign(destinationPropertyExpression,
                                    Expression.Call(Expression.Property(sourceParameter, property.Name), "ToString",
                                        null));
                                body =
                                    Expression.Convert(
                                        Expression.Assign(destinationPropertyExpression, sourcePropertyExpression),
                                        typeof(object));
                            }
                            else
                            {
                                var sourcePropertyExpression = Expression.Assign(destinationPropertyExpression,
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
                            var sourcePropertyExpression = Expression.Property(sourceParameter, property.Name);
                            body =
                                Expression.Convert(
                                    Expression.Assign(destinationPropertyExpression, sourcePropertyExpression),
                                    typeof(object));
                        }
                        var expression = Expression.Lambda(body, sourceParameter, destinationParameter);
                        var conversionDelegate = (Func<TSource, TDestination, object>)expression.Compile();

                        conversions.Add(new Conversion
                        {
                            SourceProperty = property.Name,
                            DestinationProperty = destinationPropertyName,
                            Expression = conversionDelegate
                        });
                    }
                }
            }
            return conversions;
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
