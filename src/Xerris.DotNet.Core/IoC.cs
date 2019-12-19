using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Logging;
using Xerris.DotNet.Core.Validations;

namespace Xerris.DotNet.Core
{
    public class IoC
    {
        private IServiceProvider container;
        private bool initialized;
        private readonly object mutex = new object();

        private IoC()
        {
            Initialize();
        }

        private static IoC Instance => new IoC();
        
        private void Initialize()
        {
            if (initialized) return;

            lock (mutex)
            {
                if (initialized) return;

                var collection = new ServiceCollection();
                var startup = GetImplementingType<IAppStartup>(AppDomain.CurrentDomain.GetAssemblies());
                var configuration = startup.StartUp(collection);
                LogStartup.Initialize(configuration);

                new ConfigureServiceCollection(collection).Initialize();
                container = collection.BuildServiceProvider();

                initialized = true;
            }
        }

        private TService Find<TService>()
        {
            return container.GetRequiredService<TService>();
        }

        public static TService Resolve<TService>()
        {
            return Instance.Find<TService>();
        }
        
        private static T GetImplementingType<T>(IEnumerable<Assembly> targetAssemblies)
        {
            var type = typeof(T);
            var searchAssemblies = targetAssemblies.Where(Filter);
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
                     name.StartsWith("netstandard", StringComparison.CurrentCultureIgnoreCase));
        }
    }
}