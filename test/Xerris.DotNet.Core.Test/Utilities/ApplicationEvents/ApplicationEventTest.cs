using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xerris.DotNet.Core.Time;
using Xerris.DotNet.Core.Utilities.ApplicationEvents;
using Xunit;

namespace Xerris.DotNet.Core.Test.Utilities.ApplicationEvents
{
    public class ApplicationEventTest : IDisposable
    {
   
        private const string User = "test user";
        
        [Fact]
        public void ShouldCaptureApplicationEventForAction()
        {
            Clock.Utc.Freeze();
            
            var sink = new TestSink();
            const string operation = "simple test";
            using (var monitor = new MonitorBuilder(sink).Begin(User, operation))
            {
                monitor.Action(DoStuff);
            }

            sink.SentEvents.Count.Should().Be(1);
            sink.SentEvents.First().User.Should().Be(User);
            sink.SentEvents.First().Operation.Should().Be(operation);
            sink.SentEvents.First().Outcome.Should().Be(Outcome.Successful);
            sink.SentEvents.First().Timestamp.Should().Be(Clock.Utc.Now);
            sink.SentEvents.First().Duration.Start.Should().Be(Clock.Utc.Now);
            sink.SentEvents.First().Duration.End.Should().Be(Clock.Utc.Now);
        }

        [Fact]
        public void ShouldCaptureApplicationEventForFunc()
        {
            Clock.Utc.Freeze();
            
            var sink = new TestSink();
            const string operation = "func test";
            using (var monitor = new MonitorBuilder(sink).Begin(User, operation))
            {
                var result = monitor.Function(ReturnStuff);
                result.Should().BeTrue();
            }

            sink.SentEvents.Count.Should().Be(1);
            sink.SentEvents.First().User.Should().Be(User);
            sink.SentEvents.First().Operation.Should().Be(operation);
            sink.SentEvents.First().Outcome.Should().Be(Outcome.Successful);
            sink.SentEvents.First().Timestamp.Should().Be(Clock.Utc.Now);
            sink.SentEvents.First().Duration.Start.Should().Be(Clock.Utc.Now);
            sink.SentEvents.First().Duration.End.Should().Be(Clock.Utc.Now);
        }
        
        [Fact]
        public async Task ShouldCaptureApplicationEventForAsyncFunc()
        {
            Clock.Utc.Freeze();
            
            var sink = new TestSink();
            const string operation = "async func test";
            using (var monitor = new MonitorBuilder(sink).Begin(User, operation))
            {
                var result = await monitor.Function(ReturnStuffAsync);
                result.Should().BeTrue();
            }

            sink.SentEvents.Count.Should().Be(1);
            sink.SentEvents.First().User.Should().Be(User);
            sink.SentEvents.First().Operation.Should().Be(operation);
            sink.SentEvents.First().Outcome.Should().Be(Outcome.Successful);
            sink.SentEvents.First().Timestamp.Should().Be(Clock.Utc.Now);
            sink.SentEvents.First().Duration.Start.Should().Be(Clock.Utc.Now);
            sink.SentEvents.First().Duration.End.Should().Be(Clock.Utc.Now);
        }
        
        [Fact]
        public void ShouldCaptureApplicationEventAndMeasureDuration()
        {

            var sink = new TestSink();
            const string operation = "duration test";
            using (var monitor = new MonitorBuilder(sink).Begin(User, operation))
            {
                var result = monitor.Function(() => ReturnStuff(2000));
                result.Should().BeTrue();
            }

            sink.SentEvents.Count.Should().Be(1);
            var actual = sink.SentEvents.First();
            actual.User.Should().Be(User);
            actual.Operation.Should().Be(operation);
            actual.Outcome.Should().Be(Outcome.Successful);
            actual.Duration.End.Subtract(actual.Duration.Start).Seconds.Should().BeGreaterOrEqualTo(2);
        }

        [Fact]
        public async Task ShouldCaptureMultipleApplicationEvents()
        {
            var sink = new TestSink();
            const string operation = "multi test";
            using (var monitor = new MonitorBuilder(sink).Begin(User, operation))
            {
                monitor.Action(DoStuff);
                
                monitor.Function(ReturnStuff).Should().BeTrue();
                
                (await monitor.Function(ReturnStuffAsync)).Should().BeTrue();
            }

            sink.SentEvents.Count.Should().Be(3);
            sink.SentEvents[0].Operation.Should().Be($"{operation}:1");
            sink.SentEvents[1].Operation.Should().Be($"{operation}:2");
            sink.SentEvents[2].Operation.Should().Be($"{operation}:3");
        }
        
        [Fact]
        public void ShouldCaptureSlowApplicationEvent()
        {
            var sink = new TestSink();
            const string operation = "slow test";
            using (var monitor = new MonitorBuilder(sink).Begin(User, operation, acceptableDuration: 1))
            {
                
                var result = monitor.Function(() => ReturnStuff(2000));
                result.Should().BeTrue();
            }

            sink.SentEvents.Count.Should().Be(1);
            var actual = sink.SentEvents.First();
            actual.User.Should().Be(User);
            actual.Operation.Should().Be(operation);
            actual.Outcome.Should().Be(Outcome.Slow);
        }
        
        [Fact]
        public void ShouldCaptureFailedApplicationEvent()
        {
            var sink = new TestSink();
            const string operation = "slow test";

            string expectedMessage = null;
            try
            {
                using (var monitor = new MonitorBuilder(sink).Begin(User, operation, acceptableDuration: 1))
                {
                    monitor.Action(BreakStuff);
                }
            }
            catch (ApplicationException e)
            {
                expectedMessage = e.Message;
            }

            sink.SentEvents.Count.Should().Be(1);
            var actual = sink.SentEvents.First();
            actual.User.Should().Be(User);
            actual.Operation.Should().Be(operation);
            actual.Outcome.Should().Be(Outcome.Failed);
            actual.FailureCause.Should().Be(expectedMessage);
        }

        private void DoStuff()
        {
            
        }

        private void BreakStuff()
        {
            throw new ApplicationException("this is a failure");
        }

        private bool ReturnStuff()
        {
            return true;
        }
        
        
        private bool ReturnStuff(int delay)
        {
            Thread.Sleep(delay);
            return true;
        }
        
        private Task<bool> ReturnStuffAsync()
        {
            return Task.FromResult(true);
        }

        public void Dispose()
        {
            Clock.Utc.Thaw();
        }
    }

    internal class TestSink : IEventSink
    {
        public readonly List<ApplicationEvent> SentEvents = new List<ApplicationEvent>();
        
        public Task SendAsync(ApplicationEvent applicationEvent)
        {
            SentEvents.Add(applicationEvent);
            return Task.CompletedTask;
        }

        public Task SendAsync(IEnumerable<ApplicationEvent> applicationEvents)
        {
            SentEvents.AddRange(applicationEvents);
            return Task.CompletedTask;
        }
    }
}