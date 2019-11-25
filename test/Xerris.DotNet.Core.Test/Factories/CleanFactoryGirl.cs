using Xerris.DotNet.Core.Core.Commands;

namespace Xerris.DotNet.Core.Test.Factories
{
    public class CleanFactoryGirl : ICommand
    {
        public void Run()
        {
            FactoryGirl.Clean();
        }
    }
}