using System;
using System.ComponentModel;
using System.Linq;
using FluentAssertions;
using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Validations;
using Xunit;

// ReSharper disable InconsistentNaming

namespace Xerris.DotNet.Core.Test.Core.Extensions
{
    public class EnumExtensionsTest
    {
        [Fact]
        public void Should_Convert_To_Enum_List()
        {
            var items = new[] { "Female", "Male" }.ToEnumList<Gender>().ToArray();

            Validate.Begin().HasExactly(items, 2, "should have 2 items").Check();
            items.Should().Contain(Gender.Female);
            items.Should().Contain(Gender.Male);
        }

        [Fact]
        public void Should_Bork_When_Attempting_To_Create_Enum_List_When_An_Item_Is_Not_Part_Of_The_Enum()
        {
            Assert.Throws<ArgumentException>(() => new[] { "Female", "Male", "kaka-poopoo" }.ToEnumList<Gender>());
        }

        [Fact]
        public void Can_Parse_From_Int_Back_To_Enum()
        {
            0.ToEnum<Gender>().Should().Be(Gender.Female);
        }

        [Fact]
        public void Can_Parse_From_String_Back_To_Enum()
        {
            "0".ToEnum<Gender>().Should().Be(Gender.Female);
        }

        [Fact]
        public void Should_Not_Convert_String()
        {
            Assert.Throws<ArgumentException>(() => "0*".ToEnumExact<Gender>());
            Assert.Throws<ArgumentException>(() => "a".ToEnumExact<Gender>());
            Assert.Throws<ArgumentException>(() => "TRUEe".ToEnumExact<Gender>());
        }

        [Fact]
        public void Should_Parse_Code_By_Description()
        {
            "FT-SN".ToEnumExact<Codes>().Should().Be(Codes.FTSN);
        }

        [Fact]
        public void Should_Convert_To_Enum_List_With_A_Description()
        {
            var items = new[] { "FT", "FT-SN", "FTSN" }.ToEnumList<Codes>().ToArray();

            Validate.Begin().HasExactly(items, 3, "has exactly 3 items");
            items.Should().Contain(Codes.FT);
            items.Should().Contain(Codes.FTSN);
            items.Should().Contain(Codes.FTSN);
        }

        [Fact]
        public void ShouldParseCodeWithWhitespaceToEnum()
        {
            const string serviceType = " F T";
            serviceType.ToEnumExact<Codes>().Should().Be(Codes.FT);
        }

        [Fact]
        public void Should_Get_Sort_Order()
        {
            SortThings.Item1.GetSortOrder().Should().Be(2);
            SortThings.Item4.GetSortOrder().Should().Be(4);
        }

        [Fact]
        public void Should_Get_List_Of_Enums()
        {
            EnumExtensions.GetEnumList<SortThings>().Should().HaveCount(4);
        }

        [Fact]
        public void Should_Get_Sorted_List_Of_Enums()
        {
            var sorted = EnumExtensions.GetSortedList<SortThings>().ToArray();
            sorted.Should().HaveCount(4);
            sorted.ToArray()[0].Should().Be(SortThings.Item3);
            sorted.ToArray()[1].Should().Be(SortThings.Item1);
            sorted.ToArray()[2].Should().Be(SortThings.Item4);
            sorted.ToArray()[3].Should().Be(SortThings.Item2);
        }

        [Fact]
        public void Should_Return_Default_For_Nullable_Enum()
        {
            const string myString = "";
            myString.ToNullableEnumExact<Gender>().Should().Be(default(Gender?));
        }

        [Fact]
        public void ShouldParseNumericStringToEnum()
        {
            "1".TryToEnum<Gender>(out var value);
            value.Should().Be(Gender.Male);
        }

        [Fact]
        public void Should_Return_Value_For_Nullable_Enum()
        {
            const string myString = "Female";
            var result = myString.ToNullableEnumExact<Gender>();

            result.Should().NotBeNull();
            result?.Should().Be(Gender.Female);
        }

        [Fact]
        public void Should_Throw_Exception_When_Trying_To_Parse_Invalid_Value()
        {
            const string myString = "notFemaleOrMale";
            Assert.Throws<ArgumentException>(() => myString.ToNullableEnumExact<Gender>());
        }

        [Fact]
        public void Should_Return_True_And_Parse_Valid_Enum_Value()
        {
            var success = "Male".TryParseToEnumExact(out Gender result);

            success.Should().BeTrue();
            result.Should().Be(Gender.Male);
        }

        [Fact]
        public void Should_Return_True_And_Parse_Valid_Lowercase_Enum_Value()
        {
            var success = "male".TryParseToEnumExact(out Gender result);

            success.Should().BeTrue();
            result.Should().Be(Gender.Male);
        }

        [Fact]
        public void Should_Return_True_And_Parse_Valid_Enum_Value_Containing_Space()
        {
            var success = " Ma le".TryParseToEnumExact(out Gender result);

            success.Should().BeTrue();
            result.Should().Be(Gender.Male);
        }

        [Fact]
        public void Should_Return_True_And_Parse_Description_Enum_Value()
        {
            var success = "FT-SN".TryParseToEnumExact(out Codes result);

            success.Should().BeTrue();
            result.Should().Be(Codes.FTSN);
        }

        [Fact]
        public void Should_Return_False_And_Default_Invalid_Enum_Value()
        {
            var success = "Bogus".TryParseToEnumExact(out Gender result);

            success.Should().BeFalse();
            result.Should().Be(Gender.Female);
        }

        [Fact]
        public void Should_Return_False_And_Default_Invalid_Numeric_Enum_Value()
        {
            var success = "99".TryParseToEnumExact(out Gender result);

            success.Should().BeFalse();
            result.Should().Be(Gender.Female);
        }

        [Fact]
        public void Should_Return_False_And_Default_Bitwise_Enum()
        {
            var success = "Male,Female".TryParseToEnumExact(out Gender result);

            success.Should().BeFalse();
            result.Should().Be(Gender.Female);
        }

        [Fact]
        public void GetSequence()
        {
            SortThings.Item1.GetAttribute<SequenceAttribute>().Sequence.Should().Be(1);
            SortThings.Item2.GetAttribute<SequenceAttribute>().Sequence.Should().Be(3);
            SortThings.Item3.GetAttribute<SequenceAttribute>().Sequence.Should().Be(2);
            SortThings.Item4.GetAttribute<SequenceAttribute>().Sequence.Should().Be(5);
        }
    }

    public enum Gender
    {
        Female = 0,
        Male = 1,
        Other = 2
    }

    public enum Codes
    {
        FT,
        [Description("FT-SN")] FTSN
    }

    public enum SortThings
    {
        [EnumOrder(Order = 2)] [Sequence(1)] Item1 = 1,

        [EnumOrder(Order = 10)] [Sequence(3)] Item2 = 2,

        [EnumOrder(Order = 1)] [Sequence(2)] Item3 = 3,

        [Sequence(5)] Item4 = 4
    }
}