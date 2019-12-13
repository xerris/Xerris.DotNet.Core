using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Xerris.DotNet.Core.Extensions;
using Xerris.DotNet.Core.Logging;

namespace Xerris.DotNet.Core
{
    public class IoC
    {
        private IServiceProvider container;
        private bool initialized;

        private IoC()
        {
            var frames = new StackTrace().GetFrames();  // get method calls (frames)
            var assembly = GetType().Assembly;
            foreach (var each in frames)
            {
                var declaringType = each.GetMethod().DeclaringType;
                if (declaringType != null && declaringType.Assembly == assembly) continue;
                var memberInfo = each.GetMethod().DeclaringType;
                if (memberInfo != null) assembly = memberInfo.Assembly;
                break;
            }
            
            Initialize(assembly);
        }

        public static IoC Instance { get; } = new IoC();

        private void Initialize(Assembly caller)
        {
            if (initialized) return;

            var collection = new ServiceCollection();
            var startup = ReflectionExtensions.GetImplementingType<IAppStartup>(caller);
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
    }
}