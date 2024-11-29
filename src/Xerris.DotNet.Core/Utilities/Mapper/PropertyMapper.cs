﻿using System;
using System.Reflection;
using Xerris.DotNet.Core.Utilities.Mapper.Converter;

namespace Xerris.DotNet.Core.Utilities.Mapper;

public class PropertyMapper<T> : IPropertyMapper
{
    private readonly IValueConverter<T> converter;
    private readonly PropertyInfo source;
    private readonly PropertyInfo target;

    public PropertyMapper(PropertyInfo source, PropertyInfo target, IValueConverter<T> converter)
    {
        this.source = source;
        this.target = target;
        this.converter = converter;
    }

    public string Source => source.Name;

    public string Target => target.Name;

    public void Apply(object src, object dest)
    {
        target.SetValue(dest, converter.Convert(GetSourceValue(src)), null);
    }

    private object GetSourceValue(object src)
    {
        return source.GetGetMethod()?.Invoke(src, Array.Empty<object>());
    }
}