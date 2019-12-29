using FluentAssertions;
using Xerris.DotNet.Core.Extensions;
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
}