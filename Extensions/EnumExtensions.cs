// Extensions/EnumExtensions.cs
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace TechStax.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attr = fi.GetCustomAttributes(typeof(DescriptionAttribute), false)
                         .Cast<DescriptionAttribute>()
                         .FirstOrDefault();
            return attr?.Description ?? value.ToString();
        }
    }
}
