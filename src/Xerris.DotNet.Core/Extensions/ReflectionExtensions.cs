using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Xerris.DotNet.Core.Validations;

namespace Xerris.DotNet.Core.Extensions
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
        public static T GetImplementingType<T>(params Assembly[] targetAssemblies)
        {
            var type = typeof(T);
            
            var searchAssemblies = targetAssemblies.Any() ? targetAssemblies : AppDomain.CurrentDomain.GetAssemblies();

            var found = searchAssemblies
                            .SelectMany(s => s.GetTypes())
                            .FirstOrDefault(tt => tt.IsClass && !tt.IsAbstract && type.IsAssignableFrom(tt));
            Validate.Begin().IsNotNull(found, "found ").Check();
            return (T) Activator.CreateInstance(found);
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