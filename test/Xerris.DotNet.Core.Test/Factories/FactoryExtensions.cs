using System;

namespace Xerris.DotNet.Core.Test.Factories
{
    public static class FactoryExtensions
    {
        public static int NextId(this object item)
        {
            return FactoryGirl.UniqueId();
        }

        public static Guid NextId<T>(this T item)
        {
            return Guid.NewGuid();
        }

        public static string UniqueName(this object item, string prefix = "Name")
        {
            return $"{prefix} {FactoryGirl.UniqueId()}";
        }
    }
}