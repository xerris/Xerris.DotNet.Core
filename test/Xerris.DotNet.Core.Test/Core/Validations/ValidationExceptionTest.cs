using FluentAssertions;
using Microsoft.CSharp.RuntimeBinder;
using Xerris.DotNet.Core.Core.Validation;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Validations
{
    public class ValidationExceptionTest
    {
        [Fact]
        public void CanConstruct()
        {
            var validationException = new ValidationException(new RuntimeBinderException("poopoo"));
            validationException.Message.Should().Be("poopoo");
        }
    }
}