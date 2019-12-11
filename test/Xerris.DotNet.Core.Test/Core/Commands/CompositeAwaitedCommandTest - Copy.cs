using System;
using FluentAssertions;
using Xerris.DotNet.Core.Core.Commands;
using Xerris.DotNet.Core.Core.Extensions;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Commands
{
    [Collection("base")]
    public class CompositeCommandTest
    {
        public CompositeCommandTest()
        {
            count = 1;
        }

        private int count;

        [Fact]
        public void CanRunMultipleCommands()
        {
            new TestCommand(() => count = 5)
                .Then(new TestCommand(() => count = 10))
                .Run();
            count.Should().Be(10);
        }
    }

    internal class TestCommand : ICommand
    {
        private readonly Action action;

        public TestCommand(Action action)
        {
            this.action = action;
        }

        public void Run()
        {
            action();
        }
    }
}