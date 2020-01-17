using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Test.Factories;
using Xerris.DotNet.Core.Validations;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Extensions
{
    [Collection("base")]
    public class UtilityExtensionsTest
    {
        [Fact]
        public void CanConvertToBase64()
        {
            var actual = FactoryGirl.Build<Person>();
            Validate.Begin()
                .ComparesTo<Person>(actual.ToJson().ToBase64().FromBase64().FromJson<Person>(), actual, AssertionExtensions.Matches);
        }

        [Fact]
        public void CanSerialize()
        {
            var actual = FactoryGirl.Build<Person>();
            Validate.Begin()
                .ComparesTo<Person>(actual.ToJson().FromJson<Person>(), actual, AssertionExtensions.Matches);
        }

        [Fact]
        public void CanZip()
        {
            var actual = FactoryGirl.Build<Person>();
            Validate.Begin()
                .ComparesTo<Person>(actual.ToJson().Zip().Unzip().FromJson<Person>(), actual, AssertionExtensions.Matches);
        }
    }
}