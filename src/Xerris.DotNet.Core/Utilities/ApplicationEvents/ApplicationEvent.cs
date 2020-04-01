using System;
using System.Threading.Tasks;
using Xerris.DotNet.Core.Time;

namespace Xerris.DotNet.Core.Utilities.ApplicationEvents
{
    public enum Outcome
    {
        Successful, Failed, Slow
    }
    
    public class ApplicationEvent
    {
        private DateTime start;
        
        public DateTime Timestamp { get; set; }
        public string User { get; set; }
        public string Operation { get; set; }
        public Outcome Outcome { get; set; }
        public string FailureCause { get; set; }
        public DateTimeRange Duration { get; set; }

        public void StartEvent()
        {
            start = Clock.Utc.Now;
        }

        public void StopEvent()
        {
            var end = Clock.Utc.Now;
            Duration = new DateTimeRange(start, end);
            Timestamp = end;
        }
        
    }
    
}