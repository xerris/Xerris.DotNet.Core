using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Xerris.DotNet.Core.DI;

public static class ServiceCollectionExtensions
{
    private static void Replace<TService, TImplementation>(
        this IServiceCollection services,
        Action<IServiceCollection> addMethod)
        where TService : class
        where TImplementation : class, TService
    {
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(TService));
        if (descriptor != null)
            services.Remove(descriptor);
        addMethod(services);
    }

    public static void ReplaceSingleton<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        => services.Replace<TService, TImplementation>(s => s.AddSingleton<TService, TImplementation>());

    public static void ReplaceScoped<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        => services.Replace<TService, TImplementation>(s => s.AddScoped<TService, TImplementation>());

    public static void ReplaceTransient<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        => services.Replace<TService, TImplementation>(s => s.AddTransient<TService, TImplementation>());

    private static void AddDefault<TService, TImplementation>(
        this IServiceCollection services, Action<IServiceCollection> addMethod)
        where TService : class where TImplementation : class, TService
         => addMethod(services);

    public static void AddDefaultSingleton<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        => services.AddDefault<TService, TImplementation>(s => s.TryAddSingleton<TService, TImplementation>());

    public static void AddDefaultScoped<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        => services.AddDefault<TService, TImplementation>(s => s.TryAddScoped<TService, TImplementation>());

    public static void AddDefaultTransient<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        => services.AddDefault<TService, TImplementation>(s => s.TryAddTransient<TService, TImplementation>());
    
    public static void RegisterModules(this IEnumerable<Assembly> assemblies, IServiceCollection services )
    {
        var modules = assemblies.GetImplementingTypes<IModule>().OrderBy(m => m.Priority);
        foreach (var module in modules)
            module.RegisterServices(services);
    }
    
    internal static T GetImplementingType<T>(this IEnumerable<Assembly> targetAssemblies)
    {
        var type = typeof(T);
        var searchAssemblies = targetAssemblies.Where(Filter);
        var found = searchAssemblies
            .SelectMany(s => s.GetTypes())
            .FirstOrDefault(tt => tt.IsClass && !tt.IsAbstract && type.IsAssignableFrom(tt));

        if (found == null) throw new ArgumentException($"Unable to find type matching '{type.Name}'");
        return (T)Activator.CreateInstance(found);
    }

    private static IEnumerable<T> GetImplementingTypes<T>(this IEnumerable<Assembly> targetAssemblies,
        bool failIfNotFound = false)
    {
        var type = typeof(T);
        var found = targetAssemblies.Where(Filter)
            .SelectMany(s => s.GetTypes())
            .Where(tt => tt.IsClass && !tt.IsAbstract && type.IsAssignableFrom(tt))
            .ToArray();

        if (found.Length != 0)
            return found.Select(Activator.CreateInstance).Cast<T>();
        if (failIfNotFound) 
            throw new ArgumentException($"Unable to find types matching '{type.Name}'");
        return Array.Empty<T>();
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
}