using System;
using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Utilities
{
    public class Endeavor
    {
        public async Task Go<TE>(Func<Task> action, Func<TE, Task> exceptionAction, int retries = 5, int delay = 500)
            where TE : Exception
        {
            var attempt = 0;
            while (attempt <= retries)
            {
                try
                {
                    await action();
                    return;
                }
                catch (Exception e)
                {
                    attempt++;
                    if (e.GetType() != typeof(TE)) throw;
                    if (retries == attempt) throw;
                    await exceptionAction((TE) e);
                    await Task.Delay(delay);
                }
            }
        }
    }
}