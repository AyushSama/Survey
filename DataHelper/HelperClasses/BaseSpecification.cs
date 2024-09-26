using System.Linq.Expressions;

namespace DataHelper.HelperClasses
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; set; } = new List<string>();
        public List<Expression<Func<T, object>>> OrderBys { get; set; }
        public List<Expression<Func<T, object>>> OrderByDescendings { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPagingEnabled { get; set; } = false;
        public List<Expression<Func<T, bool>>> CriteriaList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Expression<Func<T, bool>>> Buildcriteria(T obj)
        {
            Dictionary<string, object> columns = new();
            foreach (System.Reflection.PropertyInfo prop in obj.GetType().GetProperties())
                columns.Add(prop.Name, prop.GetValue(obj, null));
            List<Expression<Func<T, bool>>> criterialist = new();
            string col;
            foreach (KeyValuePair<string, object> c in columns)
            {
                col = c.Key;
                if ((col != null) && (string.IsNullOrWhiteSpace(Convert.ToString(c.Value))))
                {
                    object value = c.Value;
                    criterialist.Add(dr => col == Convert.ToString(value));
                }
            }
            return criterialist;
        }
    }
}


