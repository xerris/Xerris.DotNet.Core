using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Core
{
    public class CompositeStrategy<T> : IStrategy<T>
    {
        private readonly IEnumerable<IStrategy<T>> tasks;
        
        public CompositeStrategy(IStrategy<T> strategy, IStrategy<T> next)
        {
            tasks = new[] {strategy, next};
        }
        
        public CompositeStrategy(params IStrategy<T>[] strategies)
        {
            tasks = strategies;
        }
        
        public async Task<T> RunAsync(T subject)
        {
            foreach (var each in tasks)
            {
                await each.RunAsync(subject);
            }
            return subject; 
        }

        public IEnumerable<IStrategy<T>> Tasks => tasks;
    }
}