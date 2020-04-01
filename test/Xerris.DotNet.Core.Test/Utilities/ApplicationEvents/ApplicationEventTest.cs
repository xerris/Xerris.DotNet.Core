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

            sink.EventsSendSynchronously.Count.Should().Be(1);
            sink.EventsSendSynchronously.First().User.Should().Be(User);
            sink.EventsSendSynchronously.First().Operation.Should().Be(operation);
            sink.EventsSendSynchronously.First().Outcome.Should().Be(Outcome.Successful);
            sink.EventsSendSynchronously.First().Timestamp.Should().Be(Clock.Utc.Now);
            sink.EventsSendSynchronously.First().Duration.Start.Should().Be(Clock.Utc.Now);
            sink.EventsSendSynchronously.First().Duration.End.Should().Be(Clock.Utc.Now);
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

            sink.EventsSendSynchronously.Count.Should().Be(1);
            sink.EventsSendSynchronously.First().User.Should().Be(User);
            sink.EventsSendSynchronously.First().Operation.Should().Be(operation);
            sink.EventsSendSynchronously.First().Outcome.Should().Be(Outcome.Successful);
            sink.EventsSendSynchronously.First().Timestamp.Should().Be(Clock.Utc.Now);
            sink.EventsSendSynchronously.First().Duration.Start.Should().Be(Clock.Utc.Now);
            sink.EventsSendSynchronously.First().Duration.End.Should().Be(Clock.Utc.Now);
        }
        
        [Fact]
        public async Task ShouldCaptureApplicationEventForAsyncFunc()
        {
            Clock.Utc.Freeze();
            
            var sink = new TestSink();
            const string operation = "async func test";
            using (var monitor = new MonitorBuilder(sink).BeginAsync(User, operation))
            {
                var result = await monitor.Function(ReturnStuffAsync);
                result.Should().BeTrue();
            }

            sink.EventsSendAsynchronously.Count.Should().Be(1);
            sink.EventsSendAsynchronously.First().User.Should().Be(User);
            sink.EventsSendAsynchronously.First().Operation.Should().Be(operation);
            sink.EventsSendAsynchronously.First().Outcome.Should().Be(Outcome.Successful);
            sink.EventsSendAsynchronously.First().Timestamp.Should().Be(Clock.Utc.Now);
            sink.EventsSendAsynchronously.First().Duration.Start.Should().Be(Clock.Utc.Now);
            sink.EventsSendAsynchronously.First().Duration.End.Should().Be(Clock.Utc.Now);
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

            sink.EventsSendSynchronously.Count.Should().Be(1);
            var actual = sink.EventsSendSynchronously.First();
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
            using (var monitor = new MonitorBuilder(sink).BeginAsync(User, operation))
            {
                monitor.Action(DoStuff);
                
                monitor.Function(ReturnStuff).Should().BeTrue();
                
                (await monitor.Function(ReturnStuffAsync)).Should().BeTrue();
            }

            sink.EventsSendAsynchronously.Count.Should().Be(3);
            sink.EventsSendAsynchronously[0].Operation.Should().Be(operation);
            sink.EventsSendAsynchronously[1].Operation.Should().Be($"{operation}:2");
            sink.EventsSendAsynchronously[2].Operation.Should().Be($"{operation}:3");
        }
        
        
        private void DoStuff()
        {
            
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
        public readonly List<ApplicationEvent> EventsSendSynchronously = new List<ApplicationEvent>();
        public readonly List<ApplicationEvent> EventsSendAsynchronously = new List<ApplicationEvent>();
            
        
        public void Send(ApplicationEvent applicationEvent)
        {
            EventsSendSynchronously.Add(applicationEvent);
        }

        public Task SendAsync(ApplicationEvent applicationEvent)
        {
            EventsSendAsynchronously.Add(applicationEvent);
            return Task.CompletedTask;
        }
    }
}