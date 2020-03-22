using System;
using Xerris.DotNet.Core.Commands;
using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Test.Factories;
using Xunit;

namespace Xerris.DotNet.Core.Test
{
    public class ModelFixture : IDisposable
    {
        public ModelFixture()
        {
            new ClearFactory().Then(new TestModelFactory()).Run();
        }
        
        public void Dispose()
        {
            FactoryGirl.Clear();
        }
    }

    public class ClearFactory : ICommand
    {
        public void Run()
        {
            FactoryGirl.Clear();
        }
    }
    
    [CollectionDefinition("Test Models")]
    public class ModelCollection : ICollectionFixture<ModelFixture>
    {
    }
}