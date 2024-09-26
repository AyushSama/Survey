using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Transactions;

namespace DataHelper.HelperClasses
{
    public static class NoLockExtension
    {
        public static async Task<List<T>> ToListWithNoLockAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default, Expression<Func<T, bool>> expression = null)
        {
            List<T> result = default;
            using (var scope = CreateTrancation())
            {
                if (expression != null)
                {
                    query = query.Where(expression);
                }
                result = await query.ToListAsync(cancellationToken);
                scope.Complete();
            }
            return result;
        }
        private static TransactionScope CreateTrancation()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                                        new TransactionOptions()
                                        {
                                            IsolationLevel = IsolationLevel.ReadUncommitted
                                        },
                                       TransactionScopeAsyncFlowOption.Enabled);
        }

        /// <summary>
        /// To get the list from Unit tests
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<List<TSource>> ToListExtension<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!(source is IAsyncEnumerable<TSource>))
                return Task.FromResult(source.ToList());
            return ToListWithNoLockAsync(source);
        }
        /// <summary>
        /// To get the first or default data from unit test
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task<TSource?> FirstOrDefaultAsyncExtension<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (!(source is IAsyncEnumerable<TSource>))
                return Task.FromResult(source.FirstOrDefault());

            return FirstOrDefaultAsyncWithNoLock(source);
        }
        /// <summary>
        /// Return type for FirstOrDefaultAsyncExtension
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        private static async Task<TSource?> FirstOrDefaultAsyncWithNoLock<TSource>(IQueryable<TSource> source)
        {
            await using (var enumerator = source.AsAsyncEnumerable().GetAsyncEnumerator())
            {
                if (await enumerator.MoveNextAsync())
                {
                    return enumerator.Current;
                }
                return default;
            }
        }

    }
}
