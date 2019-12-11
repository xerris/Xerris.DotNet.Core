using System.Collections.Generic;
using System.Linq;
using Xerris.DotNet.Core.Validations;

namespace Xerris.DotNet.Core.Test
{
    public static class TestExtensions
    {
        public static void HasExactly<T>(this IEnumerable<T> items, int expected)
        {
            Validate.Begin()
                .IsNotEmpty(items, "empty")
                .ContinueIfValid(x => x.IsEqual(items.Count(), expected, "items"))
                .Check();
        }
    }
}