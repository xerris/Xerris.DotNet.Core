using System;
using System.Collections.Generic;

namespace Xerris.DotNet.Core.Test.Factories
{
    public static class FactoryGirl
    {
        private static TestFactory Factory { get; set; }

        private static TestFactory Instance => Factory ??= new TestFactory();

        public static T Build<T>()
        {
            return Build<T>(x => { });
        }

        public static T Build<T>(Action<T> propertyUpdates)
        {
            return Instance.Build(propertyUpdates);
        }

        public static void Define<T>(Func<T> factory)
        {
            Instance.Define(factory);
        }

        public static void Clear()
        {
            Factory = null;
        }

        public static int UniqueId(string key = "anonymous")
        {
            return Instance.UniqueId(key);
        }

        public static string UniqueIdStr(string key = "anonymous")
        {
            return UniqueId(key).ToString();
        }
    }

    public class TestFactory
    {
        private readonly IDictionary<Type, Func<object>> factories = new Dictionary<Type, Func<object>>();
        private readonly IDictionary<string, int> uniqueIds = new Dictionary<string, int>();

        public T Build<T>(Action<T> propertyUpdates)
        {
            if (factories.ContainsKey(typeof(T)) == false)
                throw new ArgumentException($"Unknown entity type requested: {typeof(T).Name}");

            var entity = (T)factories[typeof(T)]();
            propertyUpdates(entity);
            return entity;
        }

        public void Define<T>(Func<T> factory)
        {
            factories[typeof(T)] = () => factory();
        }

        public int UniqueId(string key = "anonymous")
        {
            if (!uniqueIds.ContainsKey(key))
                uniqueIds.Add(key, 0);
            return uniqueIds[key] += 1;
        }
    }
}