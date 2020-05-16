namespace Xerris.DotNet.Core.Utilities.Mapper
{
    public interface IPropertyMapper
    {
        void Apply(object src, object dest);
        string Source { get; }
        string Target { get; }
    }
}
