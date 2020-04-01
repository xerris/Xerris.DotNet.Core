namespace Xerris.DotNet.Core.Utilities.ApplicationEvents
{
    public class MonitorBuilder : IMonitorBuilder
    {
        private readonly IEventSink sink;

        public MonitorBuilder(IEventSink sink)
        {
            this.sink = sink;
        }

        public IEventMonitor Begin(string user, string operation)
        {
            return new EventMonitor(user, operation, ae => sink.Send(ae));
        }

        public IEventMonitor BeginAsync(string user, string operation)
        {
            return new EventMonitor(user, operation, async ae => await sink.SendAsync(ae));
        }
    }
}