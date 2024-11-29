using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Strategies;

public class CompositeStrategy<T> : IStrategy<T>
{
    public CompositeStrategy(IStrategy<T> strategy, IStrategy<T> next)
    {
        Tasks = new[] { strategy, next };
    }

    public CompositeStrategy(params IStrategy<T>[] strategies)
    {
        Tasks = strategies;
    }

    public IEnumerable<IStrategy<T>> Tasks { get; }

    public async Task<T> RunAsync(T subject)
    {
        foreach (var each in Tasks)
            await each.RunAsync(subject);
        return subject;
    }
}