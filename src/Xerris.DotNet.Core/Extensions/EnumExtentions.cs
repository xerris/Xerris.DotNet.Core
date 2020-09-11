using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Xerris.DotNet.Core.Extensions
{
public static class EnumExtensions
    {
        public static int IntValue(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        public static IEnumerable<T> ToEnumExact<T>(this IEnumerable<string> input) where T : struct, IConvertible
        {
            return input.Select(ToEnumExact<T>);
        }

        /// <summary>
        /// Attempts to parse input string into a single enum value. Tries to be flexible by ignoring case, whitespace, and falling back to the enum Description attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns>Enum of Type T or throws an ArgumentException if no Enum value is found.</returns>
        public static T ToEnumExact<T>(this string input) where T : struct, IConvertible
        {
            if (TryParseToEnumExact(input, out T enumValue))
                return enumValue;
            throw new ArgumentException($"Unable to convert '{input}' to an Enum of type {typeof(T).Name}.");
        }

        public static T ToEnumExactOrDefault<T>(this string input, T defaultValue) where T : struct, IConvertible
        {
            return TryParseToEnumExact(input, out T enumValue) ? enumValue : defaultValue;
        }

        public static T? ToNullableEnumExact<T>(this string input) where T : struct, IConvertible
        {
            return input.RemoveWhitespace() == string.Empty ? default(T?) : input.ToEnumExact<T>();
        }

        /// <summary>
        /// Attempts to parse input string into a single enum value. Tries to be flexible by ignoring case, whitespace, and falling back to the enum Description attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryParseToEnumExact<T>(this string input, out T value) where T : struct, IConvertible
        {
            var cleanInput = input.RemoveWhitespace().ToLower();
            foreach (var enumValue in Enum.GetValues(typeof (T)).Cast<T>())
            {
                if (cleanInput != enumValue.ToString().ToLower()) continue;
                value = enumValue;
                return true;
            }

            return TryParseFromDescription(input, out value);
        }

        public static List<T> GetEnumList<T>() where T : struct, IConvertible
        {
            var values = (T[])Enum.GetValues(typeof(T));
            return new List<T>(values);
        }

        public static T ToEnum<T>(this string input) where T : IConvertible
        {
            return (T)Enum.Parse(typeof(T), input);
        }     
        
        public static bool TryToEnum<T>(this string input, out T value) where T : IConvertible
        {
            value = default;
            var isInt = int.TryParse(input.RemoveWhitespace(), out var intValue);
            if (!isInt) return false;
            
            foreach (var each in Enum.GetValues(typeof (T)))
            {
                if ((int) each != intValue) continue;
                value = (T) each;
                return true;
            }

            return false;
        }        
        
        public static T ToEnum<T>(this int input) where T : IConvertible
        {
            return (T)Enum.Parse(typeof(T), input.ToString());
        }

        public static T ToEnum<T>(this long input) where T : IConvertible
        {
            return (T)Enum.Parse(typeof(T), input.ToString());
        }

        public static IEnumerable<T> ToEnumList<T>(this IEnumerable<string> values) where T : struct, IConvertible
        {
            return values == null ? new T[0] : values.Select(ToEnumExact<T>).ToArray();
        }

        private static bool TryParseFromDescription<T>(string input, out T value) where T : struct, IConvertible
        {
            value = default;
            var fields = typeof(T).GetFields();
            foreach (var field in fields)
            {
                var descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttribute == null || descriptionAttribute.Description != input) continue;
                value = (T) Enum.Parse(typeof(T), field.Name);
                return true;
            }
            return false;
        }
        
        public static int GetSortOrder(this Enum value) 
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (EnumOrderAttribute[])fi.GetCustomAttributes(typeof(EnumOrderAttribute), false);
            return attributes.Length > 0 ? attributes[0].Order : value.IntValue();
        }

        public static int GetSequence(this Enum value)
        {
            var sequence = value.GetAttribute<SequenceAttribute>();
            return sequence?.Sequence ?? 0;
        }

        public static T GetAttribute<T>(this Enum value) where T : Attribute
        { 
            var fi = value.GetType().GetField(value.ToString());
            return (T)fi.GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }

        public static string GetDescription<T>(this T value) where T: struct, IConvertible
        {
            if (!typeof (T).IsEnum)
            {
                throw new ArgumentException("{0} must be an enumerated type", typeof(T).Name);
            }
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
        
        public static IEnumerable<T> GetSortedList<T>() where T : struct, IConvertible
        {
            var t = typeof(T);
            return !t.IsEnum ? null : Enum.GetValues(t).Cast<Enum>().OrderBy(x => x.GetSortOrder()).Cast<T>();
        }        

        public static string GetName<T>(this Enum value)
        {
            return Enum.GetName(typeof (T), value);
        }
    }
}