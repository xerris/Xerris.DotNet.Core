using System;
using Xerris.DotNet.Core.Commands;

namespace Xerris.DotNet.Core.Test.Core.Commands
{
    internal class TestCommand : ICommand
    {
        private readonly Action action;

        public TestCommand(Action action)
            => this.action = action;

        public void Run()
            => action();
    }
}