namespace Xerris.DotNet.Core.Utilities.Mapper.Converter;

public class DoubleConverter : AbstractValueConverter<double?>
{
    protected override double? InternalConvert(string input)
    {
        if (input == null) return null;
        return string.IsNullOrEmpty(input) ? null : double.Parse(input);
    }
}