using Xerris.DotNet.Core.Commands;
using Xerris.DotNet.Core.Test.Core;
using Xerris.DotNet.Core.Test.Factories;
using Xerris.DotNet.Core.Test.Model;

namespace Xerris.DotNet.Core.Test
{
    public class TestModelFactory : ICommand
    {
        public void Run()
        {
            FactoryGirl.Define(() => new Person
            {
                FirstName = "Angelina",
                LastName = "Jolie",
                Age = 25,
                SocialSecurityNumber = 12345
            });
        }
    }
}