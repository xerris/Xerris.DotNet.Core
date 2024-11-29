using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Xerris.DotNet.Core.Extensions;

namespace Xerris.DotNet.Core.Validations;

[Serializable]
[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
public sealed class MultiException : ValidationException
{
    private readonly ValidationException[] innerExceptions;

    public MultiException(string message, ValidationException innerException)
        : base(message, innerException)
    {
        innerExceptions = [innerException];
    }

    public MultiException(IEnumerable<ValidationException> innerExceptions)
        : this(null, innerExceptions)
    {
    }

    public MultiException(string message, IEnumerable<ValidationException> innerExceptions)
        : base(message, innerExceptions.FirstOrDefault())
        => this.innerExceptions = innerExceptions.Where(i => i is not null).ToArray();
    
    public IEnumerable<Exception> InnerExceptions => innerExceptions;

    public override string Message
    {
        get
        {
            var builder = new StringBuilder();
            innerExceptions.ForEach(each => builder.AppendLine(each.Message));
            return builder.ToString().TrimEnd('\n', '\r');
        }
    }
}