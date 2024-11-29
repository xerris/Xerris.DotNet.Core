using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Utilities.ApplicationEvents;

public interface IEventSink
{
    Task SendAsync(ApplicationEvent applicationEvent);
    Task SendAsync(IEnumerable<ApplicationEvent> applicationEvents);
}