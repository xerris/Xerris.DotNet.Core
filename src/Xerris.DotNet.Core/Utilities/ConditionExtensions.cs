using System;

namespace Xerris.DotNet.Core.Utilities;

public static class ConditionExtensions
{
    public static bool Between<T>(this T item, T lower, T upper) where T : IComparable
    {
        return item.CompareTo(lower) >= 0 && item.CompareTo(upper) <= 0;
    }
}