using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lab4.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public static class QueryableExtensions
    {
        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IQueryable<TSource> source, Func<TSource, TKey> keySelector, bool asc)
        {
            return source.ToList().GroupBy(keySelector);
        }
    }
}
