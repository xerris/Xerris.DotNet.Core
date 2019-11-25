using Xerris.DotNet.Core.Core.Extensions;
using Xerris.DotNet.Core.Test.Factories;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core
{
    [Collection("base")]
    public class UtilityExtensionsTest
    {
        [Fact]
        public void CanConvertToBase64()
        {
            var actual = FactoryGirl.Build<Person>();
            actual.ToJson().Zip().ToBase64().FromBase64().Unzip().FromJson<Person>().Matches(actual);
        }

        [Fact]
        public void CanSerialize()
        {
            var actual = FactoryGirl.Build<Person>();
            actual.ToJson().FromJson<Person>().Matches(actual);
        }

        [Fact]
        public void CanZip()
        {
            var actual = FactoryGirl.Build<Person>();
            actual.ToJson().Zip().Unzip().FromJson<Person>().Matches(actual);
        }
    }
}