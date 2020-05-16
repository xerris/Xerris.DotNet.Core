namespace Xerris.DotNet.Core.Utilities.Mapper.Converter
{
    public class IntegerConverter : AbstractValueConverter<int?>
    {
        protected override int? InternalConvert(string input) => 
            string.IsNullOrEmpty(input) ? (int?)null : int.Parse(input);
    }
}
