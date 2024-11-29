using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Xerris.DotNet.Core.Test.Model;
using Xerris.DotNet.Core.Validations;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Validations
{
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
    public class ValidationExtensionsTest
    {
        private const string ValidationMessage = "Should not be empty";

        [Fact]
        public void Check_MultipleErrors()
        {
            var validation = new Validation();
            var error1 = new ValidationException("Foo") { Severity = ValidationSeverity.Error };
            var error2 = new ValidationException("Bar") { Severity = ValidationSeverity.Error };
            validation.Add(error1);
            validation.Add(error2);

            validation.Invoking(x => x.Check())
                .Should().Throw<MultiException>().WithMessage("Foo\r\nBar");
        }

        [Fact]
        public void Check_NoErrors()
        {
            var validation = new Validation();
            validation.Check().Should().BeSameAs(validation);
        }

        [Fact]
        public void Contains()
        {
            Validate.Begin().Contains(new[] { 1 }, 1, ValidationMessage).Check();
            Validate.Begin().Contains(new[] { 2 }, 1, ValidationMessage).Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage(ValidationMessage);
        }

        [Fact]
        public void ContinueIfValidHard()
            => Validate.Begin()
                .IsTrue(true, "continue")
                .ContinueIfValid(v => v.IsTrue(true, "continue")
                    .ContinueIfValid(v1 => v1.IsTrue(false, "I should see this"))
                    .IsTrue(true, "I should not see this"))
                .IsTrue(false, "I should also see this")
                .ContinueIfValid(v => v.IsTrue(false, "Will not see this"))
                .Invoking(x => x.Check()).Should().Throw<ValidationException>()
                .WithMessage("I should see this\nI should also see this");

        [Fact]
        public void Email_Validation_Tests()
        {
            Validate.Begin().IsEmail("dude@dude.com", "email").Check().IsValid().Should().BeTrue();
            Validate.Begin().IsEmail("e@eee.com", "email").Check().IsValid().Should().BeTrue();
            Validate.Begin().IsEmail("eee@e-e.com", "email").Check().IsValid().Should().BeTrue();
            Validate.Begin().IsEmail("eee@ee.eee.museum", "email").Check().IsValid().Should().BeTrue();

            Validate.Begin().IsEmail(".@eee.com", "invalid email").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid email");

            Validate.Begin().IsEmail("eee@e-.com", "invalid email").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid email");

            Validate.Begin().IsEmail("eee@ee.eee.eeeeeeeeee", "invalid email").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid email");
        }

        [Fact]
        public void GreaterThan_Comparable()
        {
            Validate.Begin().GreaterThan(2, 1, ValidationMessage).Check();
            Validate.Begin().GreaterThan(1, 1, ValidationMessage).Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage(ValidationMessage);
        }

        [Fact]
        public void GreaterThan_DateTime()
        {
            Validate.Begin().GreaterThan(new DateTime(2000, 1, 2), null, ValidationMessage).Check();
            Validate.Begin().GreaterThan(null, new DateTime(2000, 1, 1), ValidationMessage).Check();
            Validate.Begin()
                .GreaterThan((DateTime?)new DateTime(2000, 1, 2), new DateTime(2000, 1, 1), ValidationMessage)
                .Check();
            Validate.Begin()
                .GreaterThan((DateTime?)new DateTime(2000, 1, 1), new DateTime(2000, 1, 1), ValidationMessage)
                .Invoking(x => x.Check()).Should().Throw<ValidationException>()
                .WithMessage(ValidationMessage);
        }

        [Fact]
        public void GreaterThanOrEqual_Comparable()
        {
            Validate.Begin().GreaterThanOrEqual(2, 1, ValidationMessage).Check();
            Validate.Begin().GreaterThanOrEqual(1, 1, ValidationMessage).Check();
            Validate.Begin().GreaterThanOrEqual(0, 1, ValidationMessage).Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage(ValidationMessage);
        }

        [Fact]
        public void GreaterThanOrEqual_DateTime()
        {
            Validate.Begin().GreaterThanOrEqual(new DateTime(2000, 1, 2), null, ValidationMessage)
                .Check();
            Validate.Begin().GreaterThanOrEqual(null, new DateTime(2000, 1, 1), ValidationMessage)
                .Check();
            Validate.Begin()
                .GreaterThanOrEqual((DateTime?)new DateTime(2000, 1, 2), new DateTime(2000, 1, 1), ValidationMessage)
                .Check();
            Validate.Begin()
                .GreaterThanOrEqual((DateTime?)new DateTime(2000, 1, 1), new DateTime(2000, 1, 1), ValidationMessage)
                .Check();
            Validate.Begin()
                .GreaterThanOrEqual((DateTime?)new DateTime(2000, 1, 1), new DateTime(2000, 1, 2), ValidationMessage)
                .Invoking(x => x.Check()).Should().Throw<ValidationException>()
                .WithMessage(ValidationMessage);
        }

        [Fact]
        public void LessThan()
            => Validate.Begin()
                .LessThan(1, 2, "1 < 2").Check()
                .LessThan(2.0, 2.1, "2.0 < 2.1").Check()
                .LessThan(2.0m, 2.1m, "2.0m < 2.1m")
                .Check();


        [Fact]
        public void LessThan_DateTime()
            => Validate.Begin()
                .LessThan(new DateTime(2000, 1, 1, 14, 0, 1), new DateTime(2000, 1, 1, 14, 0, 2), "14:00:01 < 14:00:02")
                .Check()
                .LessThan(new DateTime(2000, 1, 1, 14, 1, 0), new DateTime(2000, 1, 1, 14, 2, 0), "14:01:00 < 14:02:00")
                .Check()
                .LessThan(new DateTime(2000, 1, 1), new DateTime(2000, 1, 2), "2000-01-01 < 2000-01-02")
                .Check()
                .LessThan(new DateTime(2000, 1, 1), new DateTime(2000, 2, 1), "2000-01-00 < 2000-02-00")
                .Check();

        [Fact]
        public void IsEmpty_Enumerable()
        {
            Validate.Begin().IsEmpty(new object[] { }, "Foo").Check();
            Validate.Begin().IsEmpty(new object[] { 1 }, "Foo").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("Foo should be empty");
        }

        [Fact]
        public void IsEmpty_String()
        {
            Validate.Begin().IsEmpty(string.Empty, "Foo").Check();
            Validate.Begin().IsEmpty(null, "Foo").Check();
            Validate.Begin().IsEmpty("A", "Foo").Invoking(x => x.Check()).Should().Throw<ValidationException>()
                .WithMessage("Foo should be empty");
        }

        [Fact]
        public void IsFalse()
        {
            Validate.Begin().IsFalse(false, ValidationMessage).Check();
            Validate.Begin().IsFalse(true, ValidationMessage).Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage(ValidationMessage);

            bool? nullableTrue = false;
            Validate.Begin().IsFalse(nullableTrue, ValidationMessage).Check();
            nullableTrue = null;
            Validate.Begin().IsFalse(nullableTrue, ValidationMessage).Invoking(x => x.Check()).Should()
                .Throw<ValidationException>();
        }

        [Fact]
        public void IsNotEmpty_Enumerable()
        {
            Validate.Begin().IsNotEmpty(new object[] { 1 }, "Foo").Check();
            Validate.Begin().IsNotEmpty(Array.Empty<object>(), "Foo").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("Foo is not expected to be empty");
        }

        [Fact]
        public void IsNotEmpty_String()
        {
            Validate.Begin().IsNotEmpty("A", ValidationMessage).Check();
            Validate.Begin().IsNotEmpty(string.Empty, ValidationMessage).Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage(ValidationMessage);
            Validate.Begin().IsNotEmpty(null, ValidationMessage).Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage(ValidationMessage);
        }

        [Fact]
        public void IsNotNull_DateTime()
        {
            Validate.Begin().IsNotNull(new DateTime(2000, 1, 1), ValidationMessage).Check();
            Validate.Begin().IsNotNull(DateTime.MinValue, ValidationMessage).Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage(ValidationMessage);
        }

        [Fact]
        public void IsNotNull_Decimal()
        {
            Validate.Begin().IsNotNull(1m, ValidationMessage).Check();
            Validate.Begin().IsNotNull(0m, ValidationMessage).Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage(ValidationMessage);
        }

        [Fact]
        public void IsNotNull_Int()
        {
            Validate.Begin().IsNotNull(1, ValidationMessage).Check();
            Validate.Begin().IsNotNull(0, ValidationMessage)
                .Invoking(x => x.Check())
                .Should().Throw<ValidationException>()
                .WithMessage(ValidationMessage);
        }

        [Fact]
        public void IsNotNull_NullableInt()
        {
            Validate.Begin().IsNotNull((int?)1, ValidationMessage).Check();
            Validate.Begin().IsNotNull((int?)null, ValidationMessage).Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage(ValidationMessage);
        }

        [Fact]
        public void IsNotNull_Object()
        {
            Validate.Begin().IsNotNull("A", ValidationMessage).Check();
            Validate.Begin().IsNotNull((object)null, ValidationMessage)
                .Invoking(x => x.Check())
                .Should().Throw<ValidationException>()
                .WithMessage(ValidationMessage);
        }

        [Fact]
        public void IsNull_NullableInt()
        {
            Validate.Begin().IsNull((int?)null, "Foo").Check();
            Validate.Begin().IsNull((int?)1, "Foo").Invoking(x => x.Check())
                .Should().Throw<ValidationException>()
                .WithMessage("Foo should be null");
        }

        [Fact]
        public void IsNull_Object()
        {
            Validate.Begin().IsNull(null, "Foo").Check();
            Validate.Begin().IsNull("A", "Foo").Invoking(x => x.Check())
                .Should().Throw<ValidationException>()
                .WithMessage("Foo should be null");
        }

        [Fact]
        public void IsOfType()
        {
            Validate.Begin().IsOfType<int>(1, "Foo").Check();
            Validate.Begin().IsOfType<int>("A", "Foo").Invoking(x => x.Check()).Should().Throw<ValidationException>()
                .WithMessage("Foo is not of type Int32");
            Validate.Begin().IsOfType<int>(null, "Foo").Invoking(x => x.Check()).Should().Throw<ValidationException>()
                .WithMessage("Foo");
        }

        [Fact]
        public void IsTrue()
        {
            Validate.Begin().IsTrue(true, ValidationMessage).Check();
            Validate.Begin().IsTrue(false, ValidationMessage).Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage(ValidationMessage);

            bool? nullableTrue = true;
            Validate.Begin().IsTrue(nullableTrue, ValidationMessage).Check();
            nullableTrue = null;
            Validate.Begin().IsTrue(nullableTrue, ValidationMessage).Invoking(x => x.Check()).Should()
                .Throw<ValidationException>();
        }

        [Fact]
        public void IsWithinLength()
        {
            Validate.Begin().IsWithinLength("12345", 5, "Number too big").Check();
            Validate.Begin().IsWithinLength("123456", 5, "Number too big").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("Number too big");
        }

        [Fact]
        public void Or()
        {
            new Validation().Or(ValidationMessage).Check();
            new Validation().Or(ValidationMessage, false).Invoking(x => x.Check()).Should().Throw<ValidationException>()
                .WithMessage(ValidationMessage);
            new Validation().Or(ValidationMessage, true).Invoking(x => x.Check()).Should().Throw<ValidationException>()
                .WithMessage(ValidationMessage);
        }

        [Fact]
        public void ShouldAppendMultipleValidations()
            => Validate.Begin()
                .IsNotEmpty("derp", "I will not see this")
                .Append(Validate.Begin().IsEmail("derp", "this is not an email"))
                .Append(Validate.Begin().IsNotEmpty("derp", "I will not see this"))
                .Append(Validate.Begin().IsNumeric("derp", "this is not numeric"))
                .Invoking(x => x.Check())
                .Should().Throw<ValidationException>()
                .WithMessage("this is not an email\nthis is not numeric");

        [Fact]
        public void ShouldAppendNothing()
            => Validate.Begin()
                .IsNotEmpty("derp", "I will not see this")
                .Append(Validate.Begin().IsNotEmpty("derp", "I will not see this"))
                .Append(Validate.Begin().IsNotEmpty("derp", "I will not see this"))
                .Append(Validate.Begin().IsNotEmpty("derp", "I will not see this"))
                .Check();

        [Fact]
        public void ShouldAppendValidations()
            => Validate.Begin()
                .IsNotEmpty(string.Empty, "this is empty")
                .Append(Validate.Begin().IsEmail("derp", "this is not an email"))
                .Invoking(x => x.Check())
                .Should().Throw<ValidationException>()
                .WithMessage("this is empty\nthis is not an email");

        [Fact]
        public void ShouldBeAbleToNestContinueIfValid()
            => Validate.Begin()
                .IsTrue(true, "continue")
                .ContinueIfValid(v => v.IsTrue(true, "continue")
                    .ContinueIfValid(v1 => v1.IsTrue(false, "I should see this")))
                .IsTrue(false, "I should also see this")
                .Invoking(x => x.Check())
                .Should().Throw<ValidationException>()
                .WithMessage("I should see this\nI should also see this");

        [Fact]
        public void ShouldBeAbleToValidateExactLengthOfString()
        {
            Validate.Begin().IsLengthOf("123", 3, "lengthParameter").Check();
            Validate.Begin().IsLengthOf("123", 33, "lengthParameter").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("lengthParameter needs to be exactly 33 characters long.");
            Validate.Begin().IsLengthOf("123", 2, "lengthParameter").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("lengthParameter needs to be exactly 2 characters long.");
            Validate.Begin().IsLengthOf("123", 1, "lengthParameter").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("lengthParameter needs to be exactly 1 character long.");
            Validate.Begin().IsLengthOf("", 1, "lengthParameter").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("lengthParameter needs to be exactly 1 character long.");
            Validate.Begin().IsLengthOf(null, 1, "lengthParameter").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("lengthParameter needs to be exactly 1 character long.");
        }

        [Fact]
        public void ShouldBeEquals()
            => Validate.Begin().IsEqual('C', 'C', "same characters")
                .IsEqual("AbC", "AbC", "same strings")
                .IsEqual(-123, -123, "same integers")
                .Check();

        [Fact]
        public void ShouldBeGreaterThanOrEquals()
        {
            Validate.Begin().GreaterThan(312, 311, "integer comparison").Check();
            Validate.Begin().GreaterThan('B', 'A', "character comparison").Check();
            Validate.Begin().GreaterThan("XYZ", "ABC", "string comparison").Check();
            Validate.Begin().GreaterThan(new DateTime(2018, 1, 1), new DateTime(2017, 12, 31), "DateTime comparision")
                .Check();

            Validate.Begin().GreaterThanOrEqual(312, 312, "integer comparison").Check();
            Validate.Begin().GreaterThanOrEqual('B', 'B', "character comparison").Check();
            Validate.Begin().GreaterThanOrEqual("XYZ", "XYZ", "string comparison").Check();
            Validate.Begin()
                .GreaterThanOrEqual(new DateTime(2018, 1, 1), new DateTime(2017, 12, 31), "DateTime comparision")
                .Check();
        }

        [Fact]
        public void ShouldBeInvalidPhoneNumbers()
        {
            // invalid formatting
            Validate.Begin().IsPhoneNumber("(888) 888 8888", "invalid characters in phone number")
                .Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid characters in phone number");
            Validate.Begin().IsPhoneNumber("(888) 888-8888", "invalid characters in phone number")
                .Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid characters in phone number");

            // 0-1 in 1st digit of area code not allowed
            Validate.Begin().IsPhoneNumber("088-888-8888", "invalid digit in area code").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in area code");
            Validate.Begin().IsPhoneNumber("188-888-8888", "invalid digit in area code").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in area code");

            // 0-1 in 1st digit of the 7 digit subscriber number not allowed
            Validate.Begin().IsPhoneNumber("888-088-8888", "invalid digit in subscriber number")
                .Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid digit in subscriber number");
            Validate.Begin().IsPhoneNumber("888-188-8888", "invalid digit in subscriber number")
                .Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid digit in subscriber number");

            // invalid character in area code
            Validate.Begin().IsPhoneNumber("O88-888-8888", "invalid digit in area code").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in area code");
            Validate.Begin().IsPhoneNumber("8O8-888-8888", "invalid digit in area code").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in area code");
            Validate.Begin().IsPhoneNumber("88O-888-8888", "invalid digit in area code").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in area code");
            Validate.Begin().IsPhoneNumber("8888-888-8888", "invalid digit in area code").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in area code");

            // invalid character in exchange
            Validate.Begin().IsPhoneNumber("888-O88-8888", "invalid digit in exchange number").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in exchange number");
            Validate.Begin().IsPhoneNumber("888-8O8-8888", "invalid digit in exchange number").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in exchange number");
            Validate.Begin().IsPhoneNumber("888-88O-8888", "invalid digit in exchange number").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in exchange number");
            Validate.Begin().IsPhoneNumber("888-8888-8888", "invalid digit in exchange number").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in exchange number");

            // invalid character in line number
            Validate.Begin().IsPhoneNumber("888-888-O888", "invalid digit in line number").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in line number");
            Validate.Begin().IsPhoneNumber("888-888-8O88", "invalid digit in line number").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in line number");
            Validate.Begin().IsPhoneNumber("888-888-88O8", "invalid digit in line number").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in line number");
            Validate.Begin().IsPhoneNumber("888-888-888O", "invalid digit in line number").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in line number");
            Validate.Begin().IsPhoneNumber("888-888-88888", "invalid digit in line number").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in line number");
            Validate.Begin().IsPhoneNumber("4030002345", "invalid digit in line number").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in line number");
            Validate.Begin().IsPhoneNumber("1032002345", "invalid digit in line number").Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("invalid digit in line number");
        }

        [Fact]
        public void ShouldBeInvalidPostalCodes()
        {
            // space required between forward sortation area and local delivery unit
            Validate.Begin().IsPostalCode("T3A0H8", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");

            // invalid letter in first position
            Validate.Begin().IsPostalCode("D3A 0E5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("F3A 0E5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("I3A 0E5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("O3A 0E5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("Q3A 0E5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("U3A 0E5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("W3A 0E5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("Z3A 0E5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");

            // invalid letter in third position
            Validate.Begin().IsPostalCode("A3D 0E5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("A3F 0E5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("A3I 0E5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("A3O 0E5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("A3U 0E5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");

            // invalid letter in fifth position
            Validate.Begin().IsPostalCode("A3A 0D5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("A3A 0F5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("A3A 0I5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("A3A 0O5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("A3A 0Q5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
            Validate.Begin().IsPostalCode("A3A 0U5", "invalid postal code").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid postal code");
        }

        [Fact]
        public void ShouldBeValidPhoneNumbers()
        {
            Validate.Begin().IsPhoneNumber("555 555 5555", "555 555 5555").Check().IsValid().Should().BeTrue();
            Validate.Begin().IsPhoneNumber("888-472-2222", "888-472-2222").Check().IsValid().Should().BeTrue();
        }

        [Fact]
        public void ShouldBeValidPostalCodes()
        {
            // actual postal codes
            Validate.Begin().IsPostalCode("V9A 5N2", "postal code").Check().IsValid().Should().BeTrue();
            Validate.Begin().IsPostalCode("K8N 5W6", "postal code").Check().IsValid().Should().BeTrue();
            Validate.Begin().IsPostalCode("B3K 5X5", "postal code").Check().IsValid().Should().BeTrue();
            Validate.Begin().IsPostalCode("H0H 0H0", "postal code").Check().IsValid().Should().BeTrue();

            // fake postal codes with W,Z in position 3 or 5
            Validate.Begin().IsPostalCode("T3W 0H8", "postal code").Check().IsValid().Should().BeTrue();
            Validate.Begin().IsPostalCode("T3Z 0H8", "postal code").Check().IsValid().Should().BeTrue();
            Validate.Begin().IsPostalCode("T3A 0W8", "postal code").Check().IsValid().Should().BeTrue();
            Validate.Begin().IsPostalCode("T3A 0Z8", "postal code").Check().IsValid().Should().BeTrue();
        }

        [Fact]
        public void ShouldCheckAfterContinueIfValid()
            => Validate.Begin()
                .IsTrue(true, "continue")
                .ContinueIfValid(v => v.IsTrue(false, "I should see this"))
                .IsTrue(false, "I should also see this")
                .Invoking(x => x.Check())
                .Should().Throw<ValidationException>()
                .WithMessage("I should see this\nI should also see this");

        [Fact]
        public void ShouldContainItemMatchingCriteria()
        {
            var items = new List<string> { "why hi there" };
            Validate.Begin().Contains(items, x => x == "why hi there", "error message").Check().IsValid().Should()
                .BeTrue();
            Validate.Begin().Contains(items, x => x == "hello world", "error message").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("error message");
        }

        [Fact]
        public void ShouldContinueIfValid()
            => Validate.Begin()
                .IsTrue(true, "continue")
                .ContinueIfValid(v => v.IsTrue(true, "I will not see this"))
                .Check();

        [Fact]
        public void ShouldNotBeEquals()
        {
            Validate.Begin().IsNotEqual('C', 'A', "different characters").Check();
            Validate.Begin().IsNotEqual("AbC", "Abc", "different strings").Check();
            Validate.Begin().IsNotEqual(-123, 123, "different integers").Check();
        }

        [Fact]
        public void ShouldNotBeValidAfterContinueIfValid()
            => Validate.Begin()
                .IsTrue(true, "continue")
                .ContinueIfValid(v => v.IsTrue(false, "I should see this"))
                .Invoking(x => x.Check())
                .Should().Throw<ValidationException>().WithMessage("I should see this");

        [Fact]
        public void ShouldNotContinueIfInValid()
            => Validate.Begin()
                .IsTrue(false, "stop here")
                .ContinueIfValid(v => v.IsTrue(false, "I should not see this"))
                .Invoking(x => x.Check())
                .Should()
                .Throw<ValidationException>().WithMessage("stop here");

        [Fact]
        public void ShouldNotValidateNumerics()
        {
            Validate.Begin().IsNumeric("1Z2H", "invalid non-digits").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid non-digits");
            Validate.Begin().IsNumeric("--1234", "double-Negative integer").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("double-Negative integer");
            Validate.Begin().IsNumeric("1-2-3-4", "invalid extra negative symbols").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid extra negative symbols");
            Validate.Begin().IsNumeric("1.2.3.4", "invalid extra decimal points").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid extra decimal points");
            Validate.Begin().IsNumeric("1..234", "invalid extra decimal points").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("invalid extra decimal points");
            Validate.Begin().IsNumeric("1,234", "commas not expected").Invoking(x => x.Check()).Should()
                .Throw<ValidationException>().WithMessage("commas not expected");
        }

        [Fact]
        public void ShouldUseCustomFunction()
        {
            Validate.Begin().IsTrue("theString", theString => true, "This is a custom message.").Check();
            Validate.Begin().IsTrue("theString", theString => theString.All(char.IsLetter), "This is a custom message.")
                .Check();
            Validate.Begin()
                .IsTrue("theString2", theString => theString.All(char.IsLetter), "This is a custom message.")
                .Invoking(x => x.Check()).Should().Throw<ValidationException>()
                .WithMessage("This is a custom message.");
            Validate.Begin().IsTrue("theString", theString => false, "This is a custom message.")
                .Invoking(x => x.Check()).Should().Throw<ValidationException>()
                .WithMessage("This is a custom message.");
        }

        [Fact]
        public void ShouldValidateNumerics()
        {
            Validate.Begin().IsNumeric("123", "numericValue").Check();
            Validate.Begin().IsNumeric("-123", "numericValue").Check();
            Validate.Begin().IsNumeric("+123", "positive integer").Check();
            Validate.Begin().IsNumeric("00.000", "superfluous leading zeroes on fractional decimal").Check();
            Validate.Begin().IsNumeric("3.1415926", "numericValue").Check();
            Validate.Begin().IsNumeric("00.0026", "numericValue").Check();
            Validate.Begin().IsNumeric("-0.75", "numericValue").Check();
            Validate.Begin().IsNumeric("0.75", "numericValue").Check();
            Validate.Begin().IsNumeric(".75", "no leading zero on fractional decimal").Check();
            Validate.Begin().IsNumeric("00.075", "superfluous leading zeroes on fractional decimal").Check();
            Validate.Begin().IsNumeric("0.0", "zero decimal").Check();
            Validate.Begin().IsNumeric("0", "Zero integer").Check();
            Validate.Begin().IsNumeric("-0.0", "sign on zero decimal").Check();
        }

        [Fact]
        public void Warnings_ErrorAndWarning()
        {
            var warning = new ValidationException("Foo") { Severity = ValidationSeverity.Warning };
            var error = new ValidationException("Foo") { Severity = ValidationSeverity.Error };
            var validation = new Validation();
            validation.Add(warning);
            validation.Add(error);
            validation.Warnings().Should().BeEquivalentTo(warning);
        }

        [Fact]
        public void Warnings_NullValidation() => ((Validation)null).Warnings().Should().BeNull();

        [Fact]
        public void IsDateWithTime() =>
            Validate.Begin().IsDate("2019-01-01 14:25:59", "is a date").IsValid().Should().BeTrue();

        [Fact]
        public void IsDate() => Validate.Begin().IsDate("2019-01-01", "is a date").IsValid().Should().BeTrue();

        [Fact]
        public void IsNotADate() => Validate.Begin().IsDate("derp", "is a date").IsValid().Should().BeTrue();

        [Fact]
        public void ComparesTo_SameType()
        {
            var angelina = new Foo("Angelina", "Jolie") { Age = 44 };
            var ladyGaga = new Foo("Angelina", "Gaga") { Age = 44 };

            Validate.Begin()
                .ComparesTo<Foo>(angelina, ladyGaga, (validation, actual, expected) =>
                {
                    validation.IsNotNull(actual, "actual").Check()
                        .IsNotNull(expected, "expected").Check()
                        .IsEqual(actual.FirstName, expected.FirstName, nameof(Foo.FirstName))
                        .IsNotEqual(actual.LastName, expected.LastName, nameof(Foo.LastName))
                        .IsEqual(actual.Age, expected.Age, nameof(Foo.Age))
                        .Check();
                })
                .Check();
        }

        [Fact]
        public void DoesNotCompareTo_DifferentType()
        {
            var angelina = new Foo("Angelina", "Jolie") { Age = 41 };
            var ladyGaga = new Bar("Angelina", "Gaga") { Age = 44, SocialSecurityNumber = 111321 };

            Validate.Begin()
                .ComparesTo(angelina, ladyGaga, (validation, actual, expected) =>
                {
                    validation.IsNotNull(actual, "actual").Check()
                        .IsNotNull(expected, "expected").Check()
                        .IsEqual(actual.FirstName, expected.FirstName, nameof(Foo.FirstName))
                        .IsNotEqual(actual.LastName, expected.LastName, nameof(Foo.LastName))
                        .IsNotEqual(actual.Age, expected.Age, nameof(Foo.Age))
                        .IsEqual(ladyGaga.SocialSecurityNumber, 111321, nameof(Bar.SocialSecurityNumber))
                        .Check();
                })
                .Check();
        }

        [Fact]
        public void Is()
        {
            var angelina = new Foo("Angelina", "Jolie") { Age = 41 };
            var ladyGaga = new Bar("Angelina", "Gaga") { Age = 44, SocialSecurityNumber = 111321 };

            Validate.Begin()
                .IsNotNull(angelina, "angelina").Check()
                .IsNotNull(ladyGaga, "ladyGaga").Check()
                .Is(angelina, (validation, subject) =>
                    validation.IsNotNull(subject, "subject")
                ).Check();
        }

        [Fact]
        public void Is_Invalid()
        {
            var angelina = new Foo("Angelina", "Jolie") { Age = 41 };
            var ladyGaga = new Bar("Angelina", "Gaga") { Age = 44, SocialSecurityNumber = 111321 };

            Assert.Throws<ValidationException>(() =>
                Validate.Begin()
                    .IsNotNull(angelina, "angelina").Check()
                    .IsNotNull(ladyGaga, "ladyGaga").Check()
                    .Is(angelina, (validation, subject) =>
                        validation.IsNull(subject, "subject")
                    ).Check()
            );


            Assert.Throws<ValidationException>(() =>
                Validate.Begin()
                    .IsNotNull(angelina, "angelina").Check()
                    .IsNotNull(ladyGaga, "ladyGaga").Check()
                    .Is(angelina, (validation, subject) =>
                        validation.IsNull(subject, "subject").Check()
                    ).Check()
            );
        }

        [Fact]
        public void IsInteger()
        {
            Validate.Begin().IsInteger("0", "true").Check();
            Validate.Begin().IsInteger("-1", "true").Check();
            Validate.Begin().IsInteger("-1", "true").Check();
            Validate.Begin().IsInteger("+1", "true").Check();
            Validate.Begin().IsInteger("9999", "true").Check();
        }

        [Fact]
        public void IsNotAnInteger()
        {
            Validate.Begin().IsInteger("", "false").IsValid().Should().BeFalse();
            Validate.Begin().IsInteger("a", "false").IsValid().Should().BeFalse();
            Validate.Begin().IsInteger("-1a", "false").IsValid().Should().BeFalse();
            Validate.Begin().IsInteger("a1", "false").IsValid().Should().BeFalse();
            Validate.Begin().IsInteger(" ", "false").IsValid().Should().BeFalse();
        }

        [Fact]
        public void IsDecimal()
        {
            Validate.Begin().IsDecimal("0", "true").Check();
            Validate.Begin().IsDecimal("0.", "true").Check();
            Validate.Begin().IsDecimal(".0", "true").Check();
            Validate.Begin().IsDecimal("-.0", "true").Check();
            Validate.Begin().IsDecimal("+.0", "true").Check();
            Validate.Begin().IsDecimal("-11", "true").Check();
            Validate.Begin().IsDecimal("1", "true").Check();
            Validate.Begin().IsDecimal("-1.01", "true").Check();
            Validate.Begin().IsDecimal("-1.1", "true").Check();
            Validate.Begin().IsDecimal("+1.1", "true").Check();
            Validate.Begin().IsDecimal("999900000.00000001111111", "true").Check();
        }

        [Fact]
        public void IsNotADecimal()
        {
            Validate.Begin().IsDecimal("0a", "true").IsValid().Should().BeFalse();
            Validate.Begin().IsDecimal(" ", "true").IsValid().Should().BeFalse();
            Validate.Begin().IsDecimal("", "true").IsValid().Should().BeFalse();
            Validate.Begin().IsDecimal("a.0", "true").IsValid().Should().BeFalse();
            Validate.Begin().IsDecimal("-~.0", "true").IsValid().Should().BeFalse();
        }

        [Fact]
        public void ForEach()
        {
            const string name = "Santa";
            var items = new[] { "one", "two", "three" };
            Validate.Begin()
                .IsNotEmpty(name, "name")
                .ForEach(items, (v, each) => v.IsNotEmpty(each, "each item"))
                .Check();
        }

        [Fact]
        public void ForEachFails()
        {
            const string name = "Santa";
            var items = new[] { "one", "two", string.Empty };

            Action fail = () => Validate.Begin()
                .IsNotEmpty(name, "name")
                .ForEach(items, (v, each) => v.IsNotEmpty(each, "string is null"))
                .Check();

            fail.Should().Throw<ValidationException>().WithMessage("string is null");
        }

        [Fact]
        public void ForEachFails_OnItemPriorToForEach()
        {
            var name = string.Empty;
            var items = new[] { "one", "two", "three" };

            Action fail = () => Validate.Begin()
                .IsNotEmpty(name, "name")
                .ForEach(items, (v, each) => v.IsNotEmpty(each, "string is null"))
                .Check();

            fail.Should().Throw<ValidationException>().WithMessage("name");
        }

        [Fact]
        public void ForEachFails_ListIsNullShouldNotThrowNullPointerException()
        {
            const string name = "kaka";
            string[] items = null;

            Action fail = () => Validate.Begin()
                .IsNotEmpty(name, "name")
                .ForEach(items, (v, each) => v.IsNotEmpty(each, "string is null"))
                .Check();

            fail.Should().Throw<ValidationException>().WithMessage("items is null or empty");
        }

        [Fact]
        public void Throw()
        {
            Action fail = () => Validate.Begin().Throw("this is the error");
            fail.Should().Throw<ValidationException>().WithMessage("this is the error");
        }
    }
}