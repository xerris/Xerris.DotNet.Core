using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Utilities.ApplicationEvents
{
    public interface IEventSink
    {
        void Send(ApplicationEvent applicationEvent);
        Task SendAsync(ApplicationEvent applicationEvent);
    }
}