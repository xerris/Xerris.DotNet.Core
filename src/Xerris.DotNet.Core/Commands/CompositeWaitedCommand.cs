using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Commands;

public class CompositeWaitedCommand : IWaitedCommand
{
    private readonly ICollection<IWaitedCommand> commands = new List<IWaitedCommand>();

    public async Task RunAsync()
    {
        await Task.WhenAll(commands.Select(x => x.RunAsync()));
    }

    public void Add(IWaitedCommand waitedCommand)
    {
        commands.Add(waitedCommand);
    }
}