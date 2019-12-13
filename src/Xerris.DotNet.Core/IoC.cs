using System;
using System.Collections.Generic;
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

        private IoC()
        {
            Initialize(GetType().Assembly.GetParentAssemblies());
        }

        private static IoC Instance => new IoC();
        
        private void Initialize(IEnumerable<Assembly> caller)
        {
            if (initialized) return;

            var collection = new ServiceCollection();
            var startup = GetImplementingType<IAppStartup>(caller);
            var configuration = startup.StartUp(collection);
            LogStartup.Initialize(configuration);

            new ConfigureServiceCollection(collection).Initialize();
            container = collection.BuildServiceProvider();

            initialized = true;
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
            var searchAssemblies = targetAssemblies.Any() ? targetAssemblies : AppDomain.CurrentDomain.GetAssemblies();
            var found = searchAssemblies
                .SelectMany(s => s.GetTypes())
                .FirstOrDefault(tt => tt.IsClass && !tt.IsAbstract && type.IsAssignableFrom(tt));
            Validate.Begin().IsNotNull(found, "found ").Check();
            return (T) Activator.CreateInstance(found);
        }
    }
}