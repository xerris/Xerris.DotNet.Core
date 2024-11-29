namespace Xerris.DotNet.Core.Utilities.Mapper.Converter;

public class DecimalConverter : AbstractValueConverter<decimal?>
{
    protected override decimal? InternalConvert(string input)
    {
        return string.IsNullOrEmpty(input) ? null : decimal.Parse(input);
    }
}