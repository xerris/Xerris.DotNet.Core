using Xerris.DotNet.Core.Commands;

namespace Xerris.DotNet.Core.Test.Factories
{
    public class CleanFactoryGirl : ICommand
    {
        public void Run() => FactoryGirl.Clear();
    }
}