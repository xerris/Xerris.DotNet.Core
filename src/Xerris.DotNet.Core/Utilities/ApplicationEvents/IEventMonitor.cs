using System;
using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Utilities.ApplicationEvents
{
    public interface IEventMonitor : IDisposable
    {
        void Action(Action action);
        T Function<T>(Func<T> func);
        Task<T> FunctionAsync<T>(Func<Task<T>> func);
    }
}