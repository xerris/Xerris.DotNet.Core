using System;

namespace Xerris.DotNet.Core.Utilities
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreForReflectionEqualsAttribute : Attribute
    {
        public string Reason { get; set; }

        public IgnoreForReflectionEqualsAttribute(string reason = "")
        {
            Reason = reason;
        }
    }
}