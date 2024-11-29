using System;
using System.Linq.Expressions;
using FluentAssertions;
using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Time;
using Xerris.DotNet.Core.Utilities.Mapper;
using Xerris.DotNet.Core.Utilities.Mapper.Converter;
using Xerris.DotNet.Core.Validations;
using Xunit;

namespace Xerris.DotNet.Core.Test.Utilities.Mapper
{
    public class MapperTest
    {
        [Fact]
        public void CanBuildUsingMapper()
        {
            var source =
                new SourceObj
                    { Age = 10, Date = Clock.Utc.Today, Gst = 5.0m, Misc = "hi", Money = 25.95, Name = "tset" };
            var target = new TestMapper().Build(source);
            target.Should().NotBeNull();
        }

        [Fact]
        public void CanGetDirectValueConverter()
        {
            object converter = TestMapper.GetValueConverter(s => s.Date, t => t.Date);
            Validate.Begin()
                .IsNotNull(converter, "converter")
                .TypeIsEqual<DirectConverter<DateTime?>>(converter, "converter type").Check();
        }

        [Fact]
        public void CanGetDateTimeConverter()
        {
            object converter = TestMapper.GetValueConverter(s => s.Misc, t => t.Date);
            Validate.Begin()
                .IsNotNull(converter, "converter")
                .TypeIsEqual<NullableDateTimeConverter>(converter, "converter type").Check();
        }

        [Fact]
        public void CanGetDoubleConverter()
        {
            object converter = TestMapper.GetValueConverter(s => s.Misc, t => t.Money);
            Validate.Begin()
                .IsNotNull(converter, "converter")
                .TypeIsEqual<DoubleConverter>(converter, "converter type").Check();
        }

        [Fact]
        public void CanGetDecimalConverter()
        {
            object converter = TestMapper.GetValueConverter(s => s.Misc, t => t.Gst);
            Validate.Begin()
                .IsNotNull(converter, "converter")
                .TypeIsEqual<DecimalConverter>(converter, "converter type").Check();
        }

        [Fact]
        public void CanGetIntConverter()
        {
            object converter = TestMapper.GetValueConverter(s => s.Misc, t => t.Age);
            Validate.Begin()
                .IsNotNull(converter, "converter")
                .TypeIsEqual<IntegerConverter>(converter, "converter type").Check();
        }

        [Fact]
        public void CanGetStringConverter()
        {
            object converter = TestMapper.GetValueConverter(s => s.Age, t => t.Misc);
            Validate.Begin()
                .IsNotNull(converter, "converter")
                .TypeIsEqual<StringConverter>(converter, "converter type").Check();
        }
    }

    internal class TestMapper : AbstractMapper<SourceObj, TargetObj>
    {
        public static IValueConverter<T> GetValueConverter<T>(Expression<Func<SourceObj, object>> sourceProperty,
            Expression<Func<TargetObj, T>> targetProperty)
        {
            var source = sourceProperty.GetProperty();
            var target = targetProperty.GetProperty();
            return GetValueConverter<T>(source, target);
        }

        protected override void Initialize()
        {
            Map(t => t.Date, z => z.Date);
            Map(t => t.Money, z => z.Money);
            Map(t => t.Gst, z => z.Gst);
            Map(t => t.Gst, z => z.Gst);
            Map(t => t.Age, z => z.Age);
            Map(t => t.Name, z => z.Name);
            Map(t => t.Misc, z => z.Misc);
        }

        protected override TargetObj Create()
        {
            return new TargetObj();
        }
    }

    internal class SourceObj
    {
        public DateTime? Date { get; set; }
        public double? Money { get; set; }
        public decimal? Gst { get; set; }
        public int? Age { get; set; }
        public string Name { get; set; }
        public string Misc { get; set; }
    }

    internal class TargetObj
    {
        public DateTime? Date { get; set; }
        public double? Money { get; set; }
        public decimal? Gst { get; set; }
        public int? Age { get; set; }
        public string Name { get; set; }
        public string Misc { get; set; }
    }
}