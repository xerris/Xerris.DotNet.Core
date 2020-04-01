using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xerris.DotNet.Core.Time;

namespace Xerris.DotNet.Core.Utilities.ApplicationEvents
{
    public class EventMonitor : IEventMonitor
    {
        private readonly string user;
        private readonly string operation;
        private readonly Action<ApplicationEvent> sinkAction;
        private readonly List<ApplicationEvent> list = new List<ApplicationEvent>(1);

        public EventMonitor(string user, string operation, Action<ApplicationEvent> sinkAction)
        {
            this.user = user;
            this.operation = operation;
            this.sinkAction = sinkAction;
        }

        private ApplicationEvent CreateApplicationEvent()
        {
            var ap = new ApplicationEvent {User = user, Operation = operation};
            ap.StartEvent();
            return ap;
        }

        private void EndApplicationEvent(ApplicationEvent ap)
        {
            ap.StopEvent();
            ap.Timestamp = Clock.Utc.Now;
            if (list.Count > 0)
            {
                ap.Operation = $"{ap.Operation}:{list.Count +1 }";
            }
            list.Add(ap);
        }

        public void Dispose()
        {
            if (sinkAction == null) return;
            foreach (var applicationEvent in list)
            {
                sinkAction(applicationEvent);
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