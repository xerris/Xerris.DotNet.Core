using System;

namespace Xerris.DotNet.Core.Extensions;

[AttributeUsage(AttributeTargets.Field)]
public class EnumOrderAttribute : Attribute
{
    public int Order { get; set; }
}

[AttributeUsage(AttributeTargets.Field)]
public class SequenceAttribute : Attribute
{
    public SequenceAttribute(int value)
        => Sequence = value;

    public int Sequence { get; set; }
}