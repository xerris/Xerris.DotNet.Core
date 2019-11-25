using System.Collections.Generic;
using Xerris.DotNet.Core.Core.Commands;

namespace Xerris.DotNet.Core.Core.Extensions
{
    public static class CommandExtensions
    {
        public static ICommand Then(this ICommand left, ICommand right)
        {
            var command = new CompositeCommand();
            command.Add(left);
            command.Add(right);
            return command;
        }

        public static ICommand<T> Then<T>(this ICommand<T> left, ICommand<T> right)
        {
            var command = new TypedCompositeCommand<T>();
            command.Add(left);
            command.Add(right);
            return command;
        }

        private class TypedCompositeCommand<T> : ICommand<T>
        {
            private readonly ICollection<ICommand<T>> commands = new List<ICommand<T>>();

            public void Run(T data)
            {
                commands.ForEach(x => x.Run(data));
            }

            public void Add(ICommand<T> command)
            {
                commands.Add(command);
            }
        }
    }
}