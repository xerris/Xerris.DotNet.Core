using Xerris.DotNet.Core.Test.Core;
using Xerris.DotNet.Core.Validations;

namespace Xerris.DotNet.Core.Test
{
    public static class AssertionExtensions
    {
        public static bool Matches(this Person actual, Person expected)
        {
            Validate.Begin()
                .IsNotNull(actual, "actual")
                .IsNotNull(expected, "expected")
                .Check()
                .IsEqual(actual.FirstName, expected.FirstName, "firstName")
                .IsEqual(actual.LastName, expected.LastName, "LastName")
                .Check();
            return true;
        }
    }
}