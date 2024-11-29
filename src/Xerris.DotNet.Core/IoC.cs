using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Xerris.DotNet.Core.Logging;

namespace Xerris.DotNet.Core;

public static class IoC
{
    private static readonly object Mutex = new();
    private static Func<IServiceCollection> serviceCollectionProvider = () => null;

    public static void ConfigureServiceCollection(IServiceCollection collection)
    {
        lock (Mutex)
        {
            serviceCollectionProvider = () => collection;
        }
    }


    public static TService Resolve<TService>()
    {
        return Singleton.Instance.Find<TService>();
    }

    public static IServiceScope CreateScope()
    {
        return Singleton.Instance.NewScope();
    }

    private class Singleton
    {
        private readonly IServiceProvider container;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Singleton()
        {
        }

        private Singleton()
        {
            var collection = serviceCollectionProvider() ?? new ServiceCollection();
            var startup = GetImplementingType<IAppStartup>(AppDomain.CurrentDomain.GetAssemblies());
            var configuration = startup.StartUp(collection);
            startup.InitializeLogging(configuration, LogStartup.Initialize);

            new ConfigureServiceCollection(collection).Initialize();
            container = collection.BuildServiceProvider();
        }

        public static Singleton Instance { get; } = new();

        private static T GetImplementingType<T>(IEnumerable<Assembly> targetAssemblies)
        {
            var type = typeof(T);
            var searchAssemblies = targetAssemblies.Where(Filter);
            var found = searchAssemblies
                .SelectMany(s => s.GetTypes())
                .FirstOrDefault(tt => tt.IsClass && !tt.IsAbstract && type.IsAssignableFrom(tt));

            if (found == null) throw new ArgumentException($"Unable to find type matching '{type.Name}'");
            return (T)Activator.CreateInstance(found);
        }

        private static bool Filter(Assembly assembly)
        {
            var name = assembly.FullName ?? string.Empty;
            return !(name.StartsWith("microsoft", StringComparison.CurrentCultureIgnoreCase) ||
                     name.StartsWith("system", StringComparison.CurrentCultureIgnoreCase) ||
                     name.StartsWith("mscorlib", StringComparison.CurrentCultureIgnoreCase) ||
                     name.StartsWith("netstandard", StringComparison.CurrentCultureIgnoreCase) ||
                     name.Contains("PresentationFramework") ||
                     name.Contains("PresentationCore")
                );
        }

        internal TService Find<TService>()
        {
            return container.GetRequiredService<TService>();
        }

        internal IServiceScope NewScope()
        {
            return container.CreateScope();
        }
    }
}