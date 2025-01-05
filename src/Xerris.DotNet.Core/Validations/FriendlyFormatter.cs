using System;
using System.Text;

namespace Xerris.DotNet.Core.Validations;

public class FriendlyFormatter
{
    public FriendlyFormatter(Exception single) => Message = $"1 - {single.Message}";

    public FriendlyFormatter(MultiException multi) => Format(multi);
    public string Message { get; private set; }

    public void Throw() => throw new ValidationException(Message);

    private void Format(MultiException multi)
    {
        var builder = new StringBuilder();
        var i = 0;
        foreach (var each in multi.InnerExceptions) builder.Append($"{i += 1} - {each.Message}\n");

        Message = builder.ToString().TrimEnd('\n', '\r');
    }
}

