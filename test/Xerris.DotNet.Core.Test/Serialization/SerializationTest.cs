using System;
using Newtonsoft.Json;
using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Serialization;
using Xerris.DotNet.Core.Time;
using Xerris.DotNet.Core.Utilities;
using Xerris.DotNet.Core.Validations;
using Xunit;

namespace Xerris.DotNet.Core.Test.Serialization
{
    public class SerializationTest
    {
        [Fact]
        public void CanSerializeAsJson()
        {
            var subject = new TestSubject { Name = "MyName", Age = 1, Start = Clock.Local.Now };
            var json = subject.ToJson();
            var from = json.FromJson<TestSubject>();

            IsEqual(from, subject);
        }

        [Fact]
        public void CanSerializeAsXml()
        {
            var subject = new TestSubject { Name = "MyName", Age = 1, Start = Clock.Local.Now };
            var xml = subject.ToXml();
            var from = xml.FromXml<TestSubject>();

            IsEqual(from, subject);
        }

        private static void IsEqual(TestSubject actual, TestSubject expected)
            => Validate.Begin()
                .IsNotNull(actual, "subject").Check()
                .IsNotNull(expected, "from").Check()
                .IsEqual(actual.Name, expected.Name, nameof(TestSubject.Name))
                .IsEqual(actual.Age, expected.Age, nameof(TestSubject.Age))
                .IsEqual(actual.Start, expected.Start, nameof(TestSubject.Start))
                .IsCloseEnough(actual.Start, expected.Start, nameof(TestSubject.Start))
                .IsEqual(actual.Id, expected.Id, nameof(TestSubject.Id))
                .IsEqual(actual.Timeline.Start, expected.Timeline.Start, nameof(TestSubject.Timeline.Start))
                .IsEqual(actual.Timeline.End, expected.Timeline.End, nameof(TestSubject.Timeline.End))
                .Check();


        public class TestSubject
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; }
            public DateTime Start { get; set; }
            public int Age { get; set; }

            [JsonConverter(typeof(DateTimeRangeConverter))]
            public DateTimeRange Timeline { get; set; } = new DateTimeRange(DateTime.Today, DateTime.Today.AddDays(1));
        }
    }
}