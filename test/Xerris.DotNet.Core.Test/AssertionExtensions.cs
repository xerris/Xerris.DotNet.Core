using Xerris.DotNet.Core.Test.Model;
using Xerris.DotNet.Core.Validations;

namespace Xerris.DotNet.Core.Test
{
    public static class AssertionExtensions
    {
        public static void Matches(Validation validation, Person actual, Person expected)
        {
            validation.IsNotNull(actual, "actual")
                .IsNotNull(expected, "expected")
                .Check()
                .IsEqual(actual.FirstName, expected.FirstName, "firstName")
                .IsEqual(actual.LastName, expected.LastName, "LastName")
                .Check();
        }
    }
}