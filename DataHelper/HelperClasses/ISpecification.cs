using System.Linq.Expressions;

namespace DataHelper.HelperClasses
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; set; }
        List<Expression<Func<T, object>>> Includes { get; set; }
        List<string> IncludeStrings { get; set; }
        List<Expression<Func<T, object>>> OrderBys { get; set; }
        List<Expression<Func<T, object>>> OrderByDescendings { get; set; }
        int Take { get; set; }
        int Skip { get; set; }
        bool IsPagingEnabled { get; set; }
    }
}
