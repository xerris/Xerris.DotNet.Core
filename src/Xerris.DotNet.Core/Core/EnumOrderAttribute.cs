using System;

namespace Xerris.DotNet.Core.Core
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumOrderAttribute : Attribute
    {
        public int Order { get; set; }
    }
}