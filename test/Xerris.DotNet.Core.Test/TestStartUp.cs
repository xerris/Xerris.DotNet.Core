using System;
using Xerris.DotNet.Core.Core.Extensions;
using Xerris.DotNet.Core.Test.Factories;

namespace Xerris.DotNet.Core.Test
{
    public class TestStartUp : IDisposable
    {
        public TestStartUp()
        {
            new CleanFactoryGirl()
                .Then(new TestModelFactory())
                .Run();
        }

        public void Dispose()
        {
            FactoryGirl.Clean();
        }
    }
}