using System;
using System.Reflection;

namespace Xerris.DotNet.Core.Utilities.Mapper
{
    public class ValueMapper<T> : IPropertyMapper
    {
        private readonly T value;
        private readonly PropertyInfo target;

        public ValueMapper(PropertyInfo target, T value)
        {
            this.value = value;
            this.target = target;
        }

        public void Apply(object src, object dest) => target.SetValue(dest, value, Array.Empty<object>());
        public string Source => Equals(default(T), value)? string.Empty : value.ToString();
        public string Target => target.Name;
    }
}
