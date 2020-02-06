using Xerris.DotNet.Core.Commands;
using Xerris.DotNet.Core.Test.Core;
using Xerris.DotNet.Core.Test.Factories;

namespace Xerris.DotNet.Core.Test
{
    public class TestModelFactory : ICommand
    {
        public void Run()
        {
            FactoryGirl.Define(() => new Person
            {
                FirstName = "Arnold",
                LastName = "Schwarzenegger"
            });
        }
    }
}