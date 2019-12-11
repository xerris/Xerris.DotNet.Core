using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Core.Commands
{
    public class CompositeWaitedCommand : IWaitedCommand
    {
        private readonly ICollection<IWaitedCommand> commands = new List<IWaitedCommand>();

        public async Task RunAsync()
        {
            foreach (var each in commands)
            {
                await each.RunAsync();
            }
        }

        public void Add(IWaitedCommand waitedCommand)
        {
            commands.Add(waitedCommand);
        }
    }
}