using System;
using Xerris.DotNet.Core.Time;

namespace Xerris.DotNet.Core.Utilities.ApplicationEvents;

public enum Outcome
{
    Successful,
    Failed,
    Slow
}

public class ApplicationEvent
{
    private DateTime start;

    public string Identifier { get; set; }
    public DateTime Timestamp { get; set; }
    public string User { get; set; }
    public string Operation { get; set; }

    public string Details { get; set; }
    public Outcome Outcome { get; set; }
    public string FailureCause { get; set; }
    public double Duration { get; set; }
    public string OperationStep { get; set; }

    public void StartEvent()
    {
        start = Clock.Utc.Now;
    }

    public void StopEvent()
    {
        var end = Clock.Utc.Now;
        Duration = (end - start).TotalMilliseconds;
        Timestamp = end;
        Identifier = Guid.NewGuid().ToString();
    }
}