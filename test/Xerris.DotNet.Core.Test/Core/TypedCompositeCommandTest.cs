using System;
using FluentAssertions;
using Xerris.DotNet.Core.Core.Commands;
using Xerris.DotNet.Core.Core.Extensions;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core
{
    [Collection("base")]
    public class TypedCompositeCommandTest
    {
        [Fact]
        public void CanRunMultipleTypedCommands()
        {
            var person = new Person {FirstName = "Martin", LastName = "Fowler"};

            new TestTypedCommand<Person>(x => x.FirstName = "Angelina")
                .Then(new TestTypedCommand<Person>(x => x.LastName = "Jolie"))
                .Run(person);

            person.FirstName.Should().Be("Angelina");
            person.LastName.Should().Be("Jolie");
        }
    }

    class TestTypedCommand<T> : ICommand<T>
    {
        private Action<T> action;

        public TestTypedCommand(Action<T> action)
        {
            this.action = action;
        }
        public void Run(T data)
        {
            action.Invoke(data);
        }
    }
}