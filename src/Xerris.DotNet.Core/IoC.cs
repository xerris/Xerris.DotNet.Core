using System;
using Microsoft.Extensions.DependencyInjection;
using Xerris.DotNet.Core.DI;
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

    public static TService Resolve<TService>() => Singleton.Instance.Find<TService>();

    public static TService TryResolve<TService, TDefault>() where TDefault : TService, new()
        => Singleton.Instance.FindOrDefault<TService, TDefault>();

    public static IServiceScope CreateScope() => Singleton.Instance.NewScope();

    public static void Reset(IServiceCollection collection = null)
    {
        lock (Mutex)
        {
            serviceCollectionProvider = () => collection ?? new ServiceCollection();
            Singleton.ResetInstance();
        }
    }

    private class Singleton
    {
        private readonly IServiceProvider container;

        static Singleton()
        {
        }

        private Singleton()
        {
            var collection = serviceCollectionProvider() ?? new ServiceCollection();
            var startup = AppDomain.CurrentDomain.GetAssemblies().GetImplementingType<IAppStartup>();
            var configuration = startup.StartUp(collection);
            startup.InitializeLogging(configuration, LogStartup.Initialize);

            // Register all modules with priority
            AppDomain.CurrentDomain.GetAssemblies().RegisterModules(collection);

            new ConfigureServiceCollection(collection).Initialize();
            container = collection.BuildServiceProvider();
        }

        public static Singleton Instance { get; private set; } = new();

        public static void ResetInstance()
        {
            Instance = new Singleton();
        }

        internal TService Find<TService>() => container.GetRequiredService<TService>();
        internal IServiceScope NewScope() => container.CreateScope();

        public TService FindOrDefault<TService, TDefault>() where TDefault : TService, new()
        {
            var service = container.GetService<TService>();
            if (service != null) return service;
            return new TDefault();
        }
    }
}