using System;
using System.Threading.Tasks;
using Serilog;

namespace Xerris.DotNet.Core.Utilities;

public class Endeavor
{
    public static async Task Go<TE>(Func<Task> action, Func<TE, Task> exceptionAction = null, int retries = 5,
        int delay = 500)
        where TE : Exception
    {
        var attempt = 0;
        while (attempt < retries)
            try
            {
                await action();
                return;
            }
            catch (Exception e)
            {
                Log.Debug("received {Exception} exception...", e.GetType().Name);

                if (e.GetType() != typeof(TE)) throw;

                attempt++;
                if (exceptionAction != null)
                    await exceptionAction((TE)e);

                if (retries == attempt)
                {
                    Log.Error("received {Exception} exception. Exceeded maximum retries", e.GetType().Name);
                    throw;
                }

                await Task.Delay(delay);
            }
    }
}