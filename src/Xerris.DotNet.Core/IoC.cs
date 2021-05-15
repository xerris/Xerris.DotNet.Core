using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xerris.DotNet.Core.Logging;

namespace Xerris.DotNet.Core
{
    public class IoC
    {
        private readonly IServiceProvider container;

        private static Func<IServiceCollection> serviceCollectionProvider = () => new ServiceCollection();
        
        private static readonly IoC Instance = new IoC();

        private IoC()
        {
            var collection = serviceCollectionProvider();
            var startup = GetImplementingType<IAppStartup>(AppDomain.CurrentDomain.GetAssemblies());
            var configuration = startup.StartUp(collection);
            startup.InitializeLogging(configuration, LogStartup.Initialize);

            new ConfigureServiceCollection(collection).Initialize();
            container = collection.BuildServiceProvider();
        }

        private TService Find<TService>()
        {
            return container.GetRequiredService<TService>();
        }

        public static void ConfigureServiceCollection(IServiceCollection collection)
        {
            serviceCollectionProvider = () => collection;
        }

        public static TService Resolve<TService>()
        {
            return Instance.Find<TService>();
        }

        public static IServiceScope CreateScope()
        {
            return Instance.container.CreateScope();
        }
        
        private static T GetImplementingType<T>(IEnumerable<Assembly> targetAssemblies)
        {
            var type = typeof(T);
            var searchAssemblies= targetAssemblies.Where(Filter);
            var found = searchAssemblies
                .SelectMany(s => s.GetTypes())
                .FirstOrDefault(tt => tt.IsClass && !tt.IsAbstract && type.IsAssignableFrom(tt));
            
            if(found == null) throw new ArgumentException($"Unable to find type matching '{type.Name}'");
            return (T) Activator.CreateInstance(found);
        }

        private static bool Filter(Assembly assembly)
        {
            var name = assembly.FullName;
            return !(name.StartsWith("microsoft", StringComparison.CurrentCultureIgnoreCase) || 
                     name.StartsWith("system", StringComparison.CurrentCultureIgnoreCase) ||
                     name.StartsWith("mscorlib", StringComparison.CurrentCultureIgnoreCase) ||
                     name.StartsWith("netstandard", StringComparison.CurrentCultureIgnoreCase) ||
                     name.Contains("PresentationFramework") ||
                     name.Contains("PresentationCore")
                     );
        }
    }

    public static class IoCExtensions
    {
        public static IServiceCollection AutoRegister(this IServiceCollection collection, Assembly assembly)
        {
            var interfaces = assembly.GetTypes().Where(x => x.IsInterface);
            foreach (var i in interfaces)
            {
                var implementingTypes = assembly.GetTypes().Where(tt => tt.IsClass && !tt.IsAbstract && i.IsAssignableFrom(tt))
                                                       .ToArray();
                if (implementingTypes.Length == 1)
                {
                    collection.TryAddSingleton(i, implementingTypes.First());
                }
            }

            return collection;
        }
    }
}