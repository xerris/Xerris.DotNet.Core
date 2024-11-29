using System.Linq;
using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Validations;
using Xunit;

namespace Xerris.DotNet.Core.Test.Core.Extensions
{
    public class ReflectionExtensionsTests
    {
        [Fact]
        public void GetImplementingTypes_Found()
        {
            var implementations = typeof(IHello).GetImplementingTypes(GetType().Assembly).ToList();
            Validate.Begin()
                .IsNotNull(implementations, "implementations")
                .Check()
                .IsNotEmpty(implementations, "implementations")
                .Check()
                .IsEqual(implementations.Count, 1, "length")
                .Check();
        }

        [Fact]
        public void GetImplementingTypes_NotFound()
        {
            var implementations = typeof(IBye).GetImplementingTypes(GetType().Assembly).ToList();
            Validate.Begin()
                .IsNotNull(implementations, "implementations")
                .Check()
                .IsEmpty(implementations, "implementations")
                .Check();
        }

        [Fact]
        public void GetParentAssemblies()
        {
            var assemblies = GetType().Assembly.GetParentAssemblies().ToList();
            Validate.Begin()
                .IsNotNull(assemblies, "assemblies")
                .Check()
                .IsEmpty(assemblies, "assemblies")
                .Check();
        }

        [Fact]
        public void GetPropertyInfo()
        {
            var firstName = ReflectionExtensions.GetProperty<Foo, string>(x => x.FirstName);
            Validate.Begin()
                .IsNotNull(firstName, "firstName PropertyInfo")
                .Check()
                .IsEqual(firstName.Name, nameof(Foo.FirstName), "nameOf property")
                .Check();
        }
    }

    public interface IHello
    {
        string Hi();
    }

    public class Hello : IHello
    {
        public string Hi()
        {
            return "HI";
        }
    }

    public interface IBye
    {
    }
}