namespace Xerris.DotNet.Core.Utilities.Mapper;

public interface IPropertyMapper
{
    string Source { get; }
    string Target { get; }
    void Apply(object src, object dest);
}