using System;
using FluentAssertions;
using Xerris.DotNet.Core.Validations;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Validations
{
    public class FriendlyFormatterTest
    {
        [Fact]
        public void ValidationException()
        {
            var single = new ValidationException("one");
            var formatted = new FriendlyFormatter(single);
            formatted.Message.Should().Be("1 - one");
        }

        [Fact]
        public void MultiWithOne()
        {
            var multi = new MultiException("one", new[] { new ValidationException("one") });
            var formatted = new FriendlyFormatter(multi);
            formatted.Message.Should().Be("1 - one");
        }

        [Fact]
        public void MultiWithTwo()
        {
            var multi = new MultiException("one", new[]
                { new ValidationException("one"), new ValidationException("two") });
            var formatted = new FriendlyFormatter(multi);
            formatted.Message.Should().Be("1 - one\n2 - two");
        }

        [Fact]
        public void MultiWithFour()
        {
            var multi = new MultiException("one", new[]
            {
                new ValidationException("one"), new ValidationException("two"),
                new ValidationException("three"), new ValidationException("four")
            });
            var formatted = new FriendlyFormatter(multi);
            formatted.Message.Should().Be("1 - one\n2 - two\n3 - three\n4 - four");
        }

        [Fact]
        public void ThrowSingle()
        {
            var exception = new ValidationException("one");
            Action act = () => new FriendlyFormatter(exception).Throw();
            act.Should().Throw<ValidationException>()
                .WithMessage("1 - one");
        }

        [Fact]
        public void ThrowMulti()
        {
            var multi = new MultiException("one", new[]
                { new ValidationException("one"), new ValidationException("two") });
            Action act = () => new FriendlyFormatter(multi).Throw();
            act.Should().Throw<ValidationException>()
                .WithMessage("1 - one\n2 - two");
        }
    }
}