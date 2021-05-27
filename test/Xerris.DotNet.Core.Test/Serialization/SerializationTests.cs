using System;
using Newtonsoft.Json;
using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Serialization;
using Xerris.DotNet.Core.Utilities;
using Xerris.DotNet.Core.Validations;
using Xunit;

namespace Xerris.DotNet.Core.Test.Serialization
{
    public class SerializationTests
    {
        [Fact]
        public void ProjectWithDateTimeRange()
        {
            var project = new Project();
            var json = project.ToJson();
            var fromJson = json.FromJson<Project>();
            Validate.Begin()
                    .IsNotNull(fromJson, "fromJson").Check()
                    .IsEqual(project.Id, fromJson.Id, nameof(Project.Id))
                    .IsEqual(project.Timeline.Start, fromJson.Timeline.Start, nameof(Project.Timeline.Start))
                    .IsEqual(project.Timeline.End, fromJson.Timeline.End, nameof(Project.Timeline.End))
                    .Check();
        }
    }


    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonConverter(typeof(DateTimeRangeConverter))]
        public DateTimeRange Timeline { get; set; } = new DateTimeRange(DateTime.Today, DateTime.Today.AddDays(1));
    }
}