using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xerris.DotNet.Core.Commands;
using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Test.Model;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Commands
{
    [Collection("base")]
    public class TypedAwaitedCompositeCommandTest
    {
        [Fact]
        public async Task CanRunMultipleTypedCommands()
        {
            var person = new Person {FirstName = "Martin", LastName = "Fowler"};

            await new TestTypedCommand<Person>(x => x.FirstName = "Angelina")
                     .Then(new TestTypedCommand<Person>(x => x.LastName = "Jolie"))
                     .RunAsync(person);

            person.FirstName.Should().Be("Angelina");
            person.LastName.Should().Be("Jolie");
        }
    }

    internal class TestTypedCommand<T> : ICommand<T>
    {
        private readonly Action<T> action;

        public TestTypedCommand(Action<T> action)
        {
            this.action = action;
        }

        public async Task RunAsync(T data)
        {
            await Task.Run(() => action(data));
        }
    }
}