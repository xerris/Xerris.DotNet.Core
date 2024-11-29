﻿namespace Xerris.DotNet.Core.Utilities.Mapper.Converter;

public class IntegerConverter : AbstractValueConverter<int?>
{
    protected override int? InternalConvert(string input)
    {
        return string.IsNullOrEmpty(input) ? null : int.Parse(input);
    }
}