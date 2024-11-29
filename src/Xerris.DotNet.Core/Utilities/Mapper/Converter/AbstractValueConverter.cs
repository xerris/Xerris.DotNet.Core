namespace Xerris.DotNet.Core.Utilities.Mapper.Converter;

public abstract class AbstractValueConverter<T> : IValueConverter<T>
{
    public virtual T Convert(object value)
        => InternalConvert(AsString(value));

    protected virtual string AsString(object value)
        => (value ?? string.Empty).ToString();

    protected abstract T InternalConvert(string input);
}