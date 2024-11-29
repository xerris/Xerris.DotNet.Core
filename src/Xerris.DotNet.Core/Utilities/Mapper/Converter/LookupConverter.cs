using System.Collections.Generic;

namespace Xerris.DotNet.Core.Utilities.Mapper.Converter;

public class LookupConverter<TFrom, TO> : IValueConverter<TO>
{
    private readonly Dictionary<TFrom, TO> conversions;
    private readonly TO defaultValue;

    public LookupConverter(TO defaultValue, Dictionary<TFrom, TO> conversions)
    {
        this.defaultValue = defaultValue;
        this.conversions = conversions;
    }

    public TO Convert(object input)
        => !(input is TFrom from) ? defaultValue : Get(from);

    private TO Get(TFrom input)
        => Equals(default(TFrom), input) || !conversions.TryGetValue(input, out var value) ? defaultValue : value;
}