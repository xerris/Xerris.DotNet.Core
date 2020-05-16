namespace Xerris.DotNet.Core.Utilities.Mapper
{
    internal interface IMapper<in TFrom, out T>
        where TFrom : class
        where T : class
    {
        T Build(TFrom message);
    }
}
