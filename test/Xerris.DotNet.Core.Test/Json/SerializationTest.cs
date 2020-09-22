using System;
using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Time;
using Xerris.DotNet.Core.Validations;
using Xunit;

namespace Xerris.DotNet.Core.Test.Json
{
    public class SerializationTest
    {
        [Fact]
        public void CanSerializeUsingDateTimeConverter()
        {
            var subject = new TestSubject {Name = "MyName", Age = 1, Start = Clock.Local.Now};
            var json = subject.ToJson();
            var from = json.FromJson<TestSubject>();

            Validate.Begin()
                .IsNotNull(subject, "subject").Check()
                .IsNotNull(from, "from").Check()
                .IsEqual(from.Name, subject.Name, nameof(TestSubject.Name))
                .IsEqual(from.Age, subject.Age, nameof(TestSubject.Age))
                .IsEqual(from.Start, subject.Start, nameof(TestSubject.Start))
                .IsCloseEnough(from.Start, subject.Start, nameof(TestSubject.Start))
                .Check();
        }
    }

    public class TestSubject
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public int Age { get; set; }
    }
}