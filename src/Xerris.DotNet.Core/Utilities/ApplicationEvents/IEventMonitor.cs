using System;
using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Utilities.ApplicationEvents;

public interface IEventMonitor : IDisposable
{
    void Action(Action action, string operationStep = null);
    T Function<T>(Func<T> func, string operationStep = null);
    Task<T> FunctionAsync<T>(Func<Task<T>> func, string operationStep = null);

    Task<int> Complete();
}