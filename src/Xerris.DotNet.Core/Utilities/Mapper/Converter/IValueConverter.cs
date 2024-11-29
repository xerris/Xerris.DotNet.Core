namespace Xerris.DotNet.Core.Utilities.Mapper.Converter;

public interface IValueConverter
{
}

public interface IValueConverter<out T> : IValueConverter
{
    T Convert(object value);
}