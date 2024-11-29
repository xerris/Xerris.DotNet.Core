using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xerris.DotNet.Core.Extensions;

namespace Xerris.DotNet.Core
{
    public static class IoCExtensions
    {
        public static IServiceCollection AutoRegister(this IServiceCollection collection, Assembly assembly)
        {
            var interfaces = assembly.GetTypes().Where(x => x.IsInterface);
            foreach (var i in interfaces)
            {
                var implementingTypes = FindAllFor(i, assembly).ToArray();
                if (implementingTypes.Length == 1)
                {
                    collection.TryAddSingleton(i, implementingTypes.First());
                }
            }

            return collection;
        }

        public static IServiceCollection AutoRegisterAll<T>(this IServiceCollection collection, Assembly assembly)
        {
            FindAllFor(typeof(T), assembly).ForEach(each => collection.AddSingleton(typeof(T), each));
            return collection;
        }

        private static IEnumerable<Type> FindAllFor(Type type, Assembly assembly)
            => assembly.GetTypes().Where(tt => tt.IsClass && !tt.IsAbstract && type.IsAssignableFrom(tt));
    }
}