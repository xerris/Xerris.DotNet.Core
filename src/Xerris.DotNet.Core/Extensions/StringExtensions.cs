using System;

namespace Xerris.DotNet.Core.Extensions;

public static class StringExtensions
{
    public static string RemoveWhitespace(this string value)
        => value?.Replace(" ", string.Empty);

    public static string Sanitize(this string value)
        => value?.Replace(Environment.NewLine, string.Empty);
}