using System.Linq;
using FluentAssertions;
using Xerris.DotNet.Core.Validations;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Validations
{
    public class MultiExceptionsTest
    {
        [Fact]
        public void OneException()
        {
            var multi = new MultiException("one", new ValidationException("one"));
            multi.InnerExceptions.Count().Should().Be(1);
            multi.Message.Should().Be("one");
        }
        
        [Fact]
        public void MultiExceptions()
        {
            var multi = new MultiException("one", 
                new[] {new ValidationException("one"),
                                     new ValidationException("two")});
            
            multi.InnerExceptions.Count().Should().Be(2);
            multi.Message.Should().Be("one\ntwo");
        }
    }
}