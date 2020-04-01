namespace Xerris.DotNet.Core.Utilities.ApplicationEvents
{
    public interface IMonitorBuilder
    {
        IEventMonitor Begin(string user, string operation);
        IEventMonitor BeginAsync(string user, string operation);
    }
}