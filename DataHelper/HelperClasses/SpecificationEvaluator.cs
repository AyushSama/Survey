using Microsoft.EntityFrameworkCore;

namespace DataHelper.HelperClasses
{
    internal class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            IQueryable<T> query = inputQuery;

            if (specification != null)
            {
                if (specification.Criteria != null)
                {
                    query = query.Where(specification.Criteria);
                }
                query = specification.Includes.Aggregate(query,
                    (current, include) => current.Include(include));

                query = specification.IncludeStrings.Aggregate(query,
                    (current, include) => current.Include(include));

                if (specification.OrderBys != null)
                {
                    query = specification.OrderBys.AsEnumerable().Reverse().Aggregate(query,
                        (current, orderBy) => current.OrderBy(orderBy));
                }
                else if (specification.OrderByDescendings != null)
                {
                    query = specification.OrderByDescendings.AsEnumerable().Reverse().Aggregate(query,
                        (current, orderByDesc) => current.OrderByDescending(orderByDesc));
                }

                if (specification.IsPagingEnabled)
                {
                    query = query.Skip(specification.Skip)
                        .Take(specification.Take);
                }
            }
            return query;
        }
    }
}
