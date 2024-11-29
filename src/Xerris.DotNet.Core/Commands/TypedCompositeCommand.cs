using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Commands;

public class TypedCompositeCommand<T> : ICommand<T>
{
    private readonly ICollection<ICommand<T>> commands = new List<ICommand<T>>();

    public async Task RunAsync(T data)
    {
        foreach (var each in commands)
            await each.RunAsync(data);
    }

    public void Add(ICommand<T> command)
    {
        commands.Add(command);
    }
}