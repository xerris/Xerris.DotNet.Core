namespace Xerris.DotNet.Core.Utilities.ApplicationEvents
{
    public interface IMonitorBuilder
    {
        IEventMonitor Begin(string user, string operation, string details = null, int acceptableDuration = 2);
    }
}