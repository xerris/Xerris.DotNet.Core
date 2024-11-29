using System;
using System.Globalization;

namespace Xerris.DotNet.Core.Utilities.Mapper.Converter;

public class NullableDateTimeConverter : AbstractValueConverter<DateTime?>
{
    public const string DateFormat = "yyyyMMdd";
    public const string DateTimeFormat = "yyyyMMdd-HH:mm:ss";

    private readonly string format;

    public NullableDateTimeConverter(string format)
        => this.format = format;

    public NullableDateTimeConverter() : this(DateFormat)
    {
    }

    public override DateTime? Convert(object value)
    {
        if (value == null || value is DateTime?)
            return (DateTime?)value;
        return base.Convert(value);
    }

    protected override DateTime? InternalConvert(string input)
        => string.IsNullOrEmpty(input)
            ? null
            : DateTime.ParseExact(input, format, CultureInfo.CurrentCulture);
}