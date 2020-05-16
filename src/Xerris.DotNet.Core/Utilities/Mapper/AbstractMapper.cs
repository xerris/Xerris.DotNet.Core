using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Utilities.Mapper.Converter;
using Xerris.DotNet.Core.Validations;

namespace Xerris.DotNet.Core.Utilities.Mapper
{
    public abstract class AbstractMapper<TFrom, TO> : IMapper<TFrom, TO>
        where TFrom : class
        where TO : class
    {
        private readonly List<IPropertyMapper> propertyMappers = new List<IPropertyMapper>();
        private readonly List<IClassMapper<TFrom, TO>> classMappers = new List<IClassMapper<TFrom, TO>>();

        private static readonly Dictionary<Type, IValueConverter> Converters =
            new Dictionary<Type, IValueConverter>
                {
                    {typeof(DateTime?), new NullableDateTimeConverter()},
                    {typeof(DateTime) , new DateTimeConverter()},
                    {typeof(string)   , new StringConverter()},
                    {typeof(decimal?) , new DecimalConverter()},
                    {typeof(double?)  , new DoubleConverter()},
                    {typeof(int?)     , new IntegerConverter()}
        }; 

        protected AbstractMapper() => InternalInitialize(); //avoids virtual call in constructor
        protected abstract void Initialize();
        protected abstract TO Create();

        private void InternalInitialize() => Initialize();

        public  List<IPropertyMapper> PropertyMappers => propertyMappers;
        public List<IClassMapper<TFrom, TO>> ClassMappers => classMappers;

        protected void Map<T>(Expression<Func<TFrom, object>> source, Expression<Func<TO, T>> target)
        {
            var sourceProperty = source.GetProperty();
            var targetProperty = target.GetProperty();
            Map(source, target, GetValueConverter<T>(sourceProperty, targetProperty));
        }

        protected static IValueConverter<T> GetValueConverter<T>(PropertyInfo source, PropertyInfo target)
                            => source.PropertyType == target.PropertyType 
                                ? new DirectConverter<T>() 
                                : GetConverterFromMap<T>();

        private static IValueConverter<T> GetConverterFromMap<T>()
        {
            var targetType = typeof(T);
            var hasConverter = Converters.TryGetValue(targetType, out var converter);
            Validate.Begin().IsTrue(hasConverter,
                $"There is no converter for type {targetType}.  Must provide a custom converter")
                    .Check();
            return (IValueConverter<T>)converter;
        }

        protected void Map<T>(Expression<Func<TFrom, object>> sourceProperty, Expression<Func<TO, T>> targetProperty, IValueConverter<T> converter)
                    => Map(sourceProperty.GetProperty(), targetProperty.GetProperty(), converter); 

        protected void Map<T>(Expression<Func<TO, T>> targetProperty, T value)
        {
            var target = targetProperty.GetProperty();
            propertyMappers.Add(new ValueMapper<T>(target, value));
        }

        protected void Map(IClassMapper<TFrom, TO> action) => classMappers.Add(action);

        private void Map<T>(PropertyInfo source, PropertyInfo target, IValueConverter<T> converter)
                    => propertyMappers.Add(new PropertyMapper<T>(source, target, converter)); 

        public TO Build(TFrom input) => BuildInternal(input, Create()); 

        private TO BuildInternal(TFrom input, TO to)
        {
            foreach (var applier in propertyMappers)
            {
                applier.Apply(input, to);
            }
            foreach (var action in classMappers)
            {
                action.Apply(input, to);
            }
            return to;
        }
    }
}
