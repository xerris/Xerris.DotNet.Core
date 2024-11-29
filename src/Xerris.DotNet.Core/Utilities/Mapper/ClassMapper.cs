using System;

namespace Xerris.DotNet.Core.Utilities.Mapper;

public class ClassMapper<TFrom, TO> : IClassMapper<TFrom, TO>
{
    private readonly Action<TFrom, TO> action;

    public ClassMapper(Action<TFrom, TO> action)
    {
        this.action = action;
    }

    public void Apply(TFrom from, TO to)
    {
        action(from, to);
    }
}