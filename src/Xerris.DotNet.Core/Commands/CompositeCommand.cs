using System.Collections.Generic;

namespace Xerris.DotNet.Core.Commands;

public class CompositeCommand : ICommand
{
    private readonly ICollection<ICommand> commands = new List<ICommand>();

    public void Run()
    {
        foreach (var each in commands)
            each.Run();
    }

    public void Add(ICommand waitedCommand)
    {
        commands.Add(waitedCommand);
    }
}