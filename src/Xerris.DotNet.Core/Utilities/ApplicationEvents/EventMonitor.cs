using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xerris.DotNet.Core.Time;

namespace Xerris.DotNet.Core.Utilities.ApplicationEvents
{
    public class EventMonitor : IEventMonitor
    {
        private readonly string user;
        private readonly string operation;
        private readonly string details;
        private readonly int acceptableDuration;
        private readonly IEventSink sink;
        private readonly List<ApplicationEvent> list = new List<ApplicationEvent>(1);

        public EventMonitor(string user, string operation, string details, int acceptableDuration, IEventSink sink)
        {
            this.user = user;
            this.operation = operation;
            this.details = details;
            this.acceptableDuration = acceptableDuration;
            this.sink = sink;
        }

        private ApplicationEvent CreateApplicationEvent()
        {
            var ap = new ApplicationEvent {User = user, Operation = operation, Details = details};
            ap.StartEvent();
            return ap;
        }

        private void EndApplicationEvent(ApplicationEvent ap)
        {
            ap.StopEvent();
            ap.Timestamp = Clock.Utc.Now;

            if (ap.Outcome == Outcome.Successful && ap.Duration.Difference().Seconds > acceptableDuration)
            {
                ap.Outcome = Outcome.Slow;
            }
            list.Add(ap);
        }

        public void Dispose()
        {
            if (sink == null) return;
            if (list.Count == 1)
            {
                sink.SendAsync(list.First());
            }
            else if (list.Count > 1)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    var ap = list[i];
                    ap.Operation = $"{ap.Operation}:{i + 1 }";
                }
                sink.SendAsync(list);
            }
        }

        public void Action(Action action)
        {
            var ap = CreateApplicationEvent();
            try
            {
                action();
                ap.Outcome = Outcome.Successful;
            }
            catch (Exception e)
            {
                ap.Outcome = Outcome.Failed;
                ap.FailureCause = e.Message;
                throw;
            }
            finally
            {
                EndApplicationEvent(ap);
            }
        }

        public T Function<T>(Func<T> func)
        {
            var ap = CreateApplicationEvent();
            try
            {
                
                var result = func();
                ap.Outcome = Outcome.Successful;
                return result;
            }
            catch (Exception e)
            {
                ap.Outcome = Outcome.Failed;
                ap.FailureCause = e.Message;
                throw;
            }
            finally
            {
                EndApplicationEvent(ap);
            }
        }

        public async Task<T> FunctionAsync<T>(Func<Task<T>> func)
        {
            var ap = CreateApplicationEvent();
            try
            {
                var result = await func();
                ap.Outcome = Outcome.Successful;
                return result;
            }
            catch (Exception e)
            {
                ap.Outcome = Outcome.Failed;
                ap.FailureCause = e.Message;
                throw;
            }
            finally
            {
                EndApplicationEvent(ap);
            }
        }
    }
}