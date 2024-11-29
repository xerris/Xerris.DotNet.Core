using Xunit;

namespace Xerris.DotNet.Core.Test
{
    [CollectionDefinition("base")]
    public class BaseTest : ICollectionFixture<ModelFixture>
    {
    }
}