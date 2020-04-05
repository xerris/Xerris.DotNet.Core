using System;

namespace Xerris.DotNet.Core.Extensions
{
    public static class ComparisonExtensions
    {
        public static bool IsCloseEnough<T>(this T left, T right, decimal tolerance) where T : IComparable<T>
        {
            return Math.Abs(left.CompareTo(right)) <= tolerance;
        }
    }
}