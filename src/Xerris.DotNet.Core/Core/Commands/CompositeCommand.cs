using System.Collections.Generic;
using Xerris.DotNet.Core.Core.Extensions;

namespace Xerris.DotNet.Core.Core.Commands
{
    public class CompositeCommand : ICommand
    {
        private readonly ICollection<ICommand> commands = new List<ICommand>();

        public void Run()
        {
            commands.ForEach(x => x.Run());
        }

        public void Add(ICommand command)
        {
            commands.Add(command);
        }
    }
}