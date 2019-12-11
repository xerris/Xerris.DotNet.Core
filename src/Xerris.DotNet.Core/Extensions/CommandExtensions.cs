using Xerris.DotNet.Core.Commands;

namespace Xerris.DotNet.Core.Extensions
{
    public static class CommandExtensions
    {
        public static IWaitedCommand Then(this IWaitedCommand left, IWaitedCommand right)
        {
            var command = new CompositeWaitedCommand();
            command.Add(left);
            command.Add(right);
            return command;
        }
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

    }
}