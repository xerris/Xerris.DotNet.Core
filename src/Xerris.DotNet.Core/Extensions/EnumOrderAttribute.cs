using System;

namespace Xerris.DotNet.Core.Extensions
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumOrderAttribute : Attribute
    {
        public int Order { get; set; }
    }
}