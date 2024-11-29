using System;

namespace Xerris.DotNet.Core.Extensions;

public static class NumericExtensions
{
    public static double RoundedTo(this double value, int precision)
    {
        return Math.Round(value, precision, MidpointRounding.AwayFromZero);
    }
}