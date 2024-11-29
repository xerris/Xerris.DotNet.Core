using System;
using System.Threading;
using Xerris.DotNet.Core.Extensions;

namespace Xerris.DotNet.Core.Time;

public static class UtcClock
{
    private static readonly ThreadLocal<IUtcService> UtcService = new(() => new UtcService());

    public static DateTime Now => UtcService.Value.UtcNow;

    public static IUtcService Freeze()
    {
        var frozenTime = DateTime.SpecifyKind(DateTime.UtcNow.ReduceMillisecondPrecision(), DateTimeKind.Utc);
        return Freeze(frozenTime);
    }

    public static IUtcService Freeze(DateTime timeToFreeze)
    {
        if (timeToFreeze.Kind != DateTimeKind.Utc) throw new Exception("Only UTC times are valid to be frozen");
        return UtcService.Value.Freeze(timeToFreeze);
    }

    public static void Thaw()
    {
        UtcService.Value.Dispose();
    }
}

public interface IUtcService : IDisposable
{
    DateTime UtcNow { get; }
    IUtcService Freeze(DateTime timeToFreeze);
}

public class UtcService : IUtcService
{
    private DateTime? frozen;

    public virtual DateTime UtcNow =>
        frozen ?? DateTime.SpecifyKind(DateTime.UtcNow.ReduceMillisecondPrecision(), DateTimeKind.Utc);

    public IUtcService Freeze(DateTime timeToFreeze)
    {
        frozen = DateTime.SpecifyKind(timeToFreeze.ReduceMillisecondPrecision(), DateTimeKind.Utc);
        return this;
    }

    public void Dispose()
    {
        frozen = null;
    }
}