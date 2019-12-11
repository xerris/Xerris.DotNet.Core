using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Xerris.DotNet.Core.Core.Extensions
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetImplementingTypes(this Type t, params Assembly[] targetAssemblies)
        {
            if (t == null) return Enumerable.Empty<Type>();

            var searchAssemblies = targetAssemblies.Any() ? targetAssemblies : AppDomain.CurrentDomain.GetAssemblies();

            return searchAssemblies
                .SelectMany(s => s.GetTypes())
                .Where(tt => tt.IsClass && !tt.IsAbstract && t.IsAssignableFrom(tt));
        }

        public static IEnumerable<Assembly> GetParentAssemblies(this Assembly a)
        {
            return new StackTrace().GetFrames()
                .Where(f => f.GetMethod().ReflectedType != null)
                .Select(f => f.GetMethod().ReflectedType.Assembly)
                .Distinct().Where(x => x.GetReferencedAssemblies().Any(y => y.FullName == a.FullName));
        }
    }
}