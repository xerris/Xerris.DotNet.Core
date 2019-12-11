using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Xerris.DotNet.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return items == null || !items.Any();
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return !items.IsNullOrEmpty();
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var each in items) action(each);
        }

        public static string BuildCsvString<T>(this IEnumerable<T> list)
        {
            return list.BuildDeliminatedString(",");
        }

        public static string BuildDeliminatedString<T>(this IEnumerable<T> list, string deliminatingItem)
        {
            return string.Join(deliminatingItem + " ", (from p in list select p).ToArray());
        }

        public static string BuildDeliminatedQuotedString<T>(this IEnumerable<T> list, string deliminatingItem)
        {
            return string.Join(deliminatingItem + " ", (from p in list select "'" + p + "'").ToArray());
        }

        public static IEnumerable<TCurrentType> NotOfType<TType, TCurrentType>(this IEnumerable<TCurrentType> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Where(obj => !(obj is TType));
        }

        public static bool DoesNotContain<T>(this IEnumerable<T> items, T item)
        {
            return !items.Contains(item);
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, params T[] tail)
        {
            return source.Concat(tail);
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            return source.Select((x, index) => new {x, index})
                .GroupBy(x => x.index / batchSize, y => y.x);
        }

        public static T MinBy<T>(this IEnumerable<T> source, Func<T, object> predicate)
        {
            return source.OrderBy(predicate).First();
        }

        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> source, Func<T, object> predicate)
        {
            return source.GroupBy(predicate).Select(x => x.First());
        }

        public static string GetEmail(this IEnumerable<string> items)
        {
            var emailRegex = new Regex(
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase);
            return items.First(x => emailRegex.IsMatch(x));
        }
    }
}