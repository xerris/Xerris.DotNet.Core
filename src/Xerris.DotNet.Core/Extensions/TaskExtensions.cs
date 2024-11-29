using Xerris.DotNet.Core.Strategies;

namespace Xerris.DotNet.Core.Extensions
{
    public static class TaskExtensions
    {
        public static IStrategy<T> Then<T>(this IStrategy<T> left, IStrategy<T> right)
            => new CompositeStrategy<T>(left, right);
    }
}