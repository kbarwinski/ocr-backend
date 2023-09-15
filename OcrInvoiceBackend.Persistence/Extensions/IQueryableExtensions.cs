using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Persistence.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int index, int size)
        {
            return query.Skip(index * size).Take(size);
        }

        public static IQueryable<T> Order<T>(this IQueryable<T> query, string sortingOrders, Dictionary<string, Expression<Func<T, object>>> orderPredicates)
        {
            var orders = sortingOrders.Split(',');

            foreach (var order in orders)
            {
                var parts = order.Trim().Split(' ');
                if (parts.Length < 2)
                    continue;

                var expression = orderPredicates.GetValueOrDefault(parts[0]);
                if (expression == null)
                    continue;

                var sortOrder = parts[1].ToLower();

                if (sortOrder.Contains("desc"))
                    query = query.OrderByDescending(expression);
                else
                    query = query.OrderBy(expression);
            }

            return query;
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> query, Dictionary<Expression<Func<T, bool>>,bool> filterPredicates)
        {
            foreach (var predicate in filterPredicates)
            {
                if (predicate.Value)
                    query = query.Where(predicate.Key);
            }
            return query;
        }
    }
}
