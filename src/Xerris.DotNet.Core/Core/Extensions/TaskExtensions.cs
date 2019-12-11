namespace Xerris.DotNet.Core.Core.Extensions
{
    public static class TaskExtensions
    {
        public static IStrategy<T> Then<T>(this IStrategy<T> left, IStrategy<T> right) 
        {
            return new CompositeStrategy<T>(left, right);
        }
    }
}