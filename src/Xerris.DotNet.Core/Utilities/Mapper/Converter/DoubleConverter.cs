namespace Xerris.DotNet.Core.Utilities.Mapper.Converter;

public class DoubleConverter : AbstractValueConverter<double?>
{
    protected override double? InternalConvert(string input)
        => string.IsNullOrEmpty(input) ? null : double.Parse(input);
}