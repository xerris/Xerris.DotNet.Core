using System;
using System.Collections.Generic;

namespace Xerris.DotNet.Core.Test.Factories
{
    public static class FactoryGirl
    {
        private static int current;

        private static readonly IDictionary<Type, Func<object>> Factories = new Dictionary<Type, Func<object>>();
        public static int NextId => current += 1;

        public static T Build<T>()
        {
            return Build<T>(x => { });
        }

        public static T Build<T>(Action<T> propertyUpdates)
        {
            if (Factories.ContainsKey(typeof(T)) == false)
                throw new ArgumentException("Unknown entity type requested: " + typeof(T).Name);
            var entity = (T) Factories[typeof(T)]();
            propertyUpdates(entity);
            return entity;
        }

        public static void Define<T>(Func<T> factory)
        {
            if (Factories.ContainsKey(typeof(T)))
                throw new ArgumentException($"{typeof(T)} has already been registered");
            Factories.Add(typeof(T), () => factory());
        }

        public static void Clean()
        {
            current = 0;
            Factories.Clear();
        }
    }

    public static class IdGenerator
    {
        private static int current = 1;

        public static int NextInt
        {
            get
            {
                current++;
                return current;
            }
        }

        public static string NextStringId => $"A-{NextInt}";

        public static void Reset()
        {
            current = 1;
        }
    }
}