using DataHelper.EFData.Common.Interfaces;
using DataHelper.Entities;
using DataHelper.Entities.EnumFields;
using DataHelper.HelperClasses;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text.Json;

namespace DataHelper.MockRepos
{

    public class MockGenericBaseRepo<T, M> : IGenericBaseRepo<T, M> where T : class, new() where M : DbContext
    {

        public MockGenericBaseRepo(M dbContext)
        {
            _dbContext = dbContext;
        }

        public M _dbContext { get; }

        #region search methods
        /// <summary>
        /// Use this method to get all columns single row return referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        public T GetUniqueRecordBySpec(ISpecification<T> spec, object message, bool asNoTracking = true)
        {
            T Entity;
            Entity = ApplySpecification(spec).FirstOrDefault();
            return Entity;
        }

        /// <summary>
        /// Use this method to get all columns all rows return without any specification criteria.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<T> ListAll(object message)
        {
            List<T> results = GetData().ToList();
            return results;
        }

        /// <summary>
        /// Use this method to get all columns multiple rows return referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<T> List(ISpecification<T> spec, object message)
        {
            List<T> results = ApplySpecification(spec).AsNoTracking().ToList();
            return results;
        }

        /// <summary>
        /// Use this method to get all columns multiple rows return referring to specification criteria passed as parameter in async way.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<List<T>> ListAsync(ISpecification<T> spec, object message)
        {
            return ApplySpecification(spec).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Use this method to get single column single value (scalar value) return referring to specification criteria and property name passed in parameter.
        /// The required column name is passed as string in propName parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="propName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public object GetSingleValue(ISpecification<T> spec, string propName, object message)
        {
            var entity = ApplySpecification(spec).AsNoTracking().FirstOrDefault();
            if (entity == null) return null;
            PropertyInfo propertyInfo = entity.GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
                return propertyInfo.GetValue(entity, null);
            return null;
        }

        /// <summary>
        /// Use this method to get single column with multiple values return referring to specification criteria and property name passed in parameter.
        /// The required column name is passed as string in propName parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="propName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public object GetSingleValueList(ISpecification<T> spec, string propName, object message)
        {
            var entity = ApplySpecification(spec).AsNoTracking().ToList();
            if (entity.Count > 0)
                return entity.AsQueryable().Select(propName);
            return null;
        }

        /// <summary>
        /// Use this method to get count of rows referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public int Count(ISpecification<T> spec, object message)
        {
            int results = ApplySpecification(spec).AsNoTracking().Count();
            return results;
        }

        /// <summary>
        /// Use this method to get count of rows referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public object Max(string columnName, object message)
        {
            var result = _dbContext.Set<T>();
            return (from d in result
                    select d).Max(columnName);

        }

        /// <summary>
        /// This is special function and should be used to get database Datetime e.g. GetDate() from database server.
        /// </summary>
        /// <returns></returns>
        public DateTime GetDBDateTime()
        {
            FormattableString query = $"SELECT getdate()";
            var dQuery = _dbContext.Database.SqlQuery<DateTime>(query);
            return dQuery.AsEnumerable().First();
        }
        #endregion

        #region add methods
        /// <summary>
        /// Use this method to insert single entity data into database table referring to entity passed as parameter.
        /// It will also have primary key in entity itself which is used as parameter while calling the add method.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public void Add(T entity, object message)
        {

        }

        /// <summary>
        /// Use this method to insert multiple entities data into database with considering specification criteria, if passed then do not insert data into database table.
        /// If criteria is not passed then insert data into database table.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Add(ISpecification<T> spec, object message)
        {
            return true;
        }

        /// <summary>
        /// Use this method to insert multiple entities data into database table referring to list of entities passed as parameter.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="message"></param>
        public void Add(List<T> entities, object message)
        {

        }
        #endregion

        #region update methods
        /// <summary>
        /// Use this method to update single entity and selected columns data into database table referring to entity, properties names & update type as parameter.
        /// When user wants to update fewer columns, pass them as array of string names in properties parameter and set UpdateType to Include.
        /// When user wants to update many columns, pass remaining columns as array of string names in properties parameter and set UpdateType to Exclude.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        /// <param name="IncludeExclude"></param>
        public void Update(T entity, object message, string[] properties, UpdateType IncludeExclude = UpdateType.Exclude)
        {

        }

        /// <summary>
        /// Use this method to update multiple entities all columns data into database table referring to specification criteria passed as parameter.
        /// User can also override the result based on criteria and it will update the result into database table
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        public void Update(ISpecification<T> spec, object message)
        {

        }

        public List<T> SetUpdateEntityResult(List<T> results)
        {
            return results;
        }


        /// <summary>
        /// Use this method to update multiple entities all columns data into database table referring to list of entities passed as parameter.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="message"></param>
        public void Update(List<T> entities, object message)
        {

        }

        /// <summary>
        /// Use this method to update single entity all columns data into database table referring to entity passed as parameter.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public void Update(T entity, object message)
        {

        }
        #endregion

        #region delete methods
        /// <summary>
        /// Use this method to delete single entity data from database table referring to single entity passed as parameter.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public void Delete(T entity, object message)
        {

        }

        /// <summary>
        /// Use this method to delete multiple entities data referring to specification criteria passed as parameter.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="message"></param>
        public void Delete(ISpecification<T> spec, object message)
        {

        }

        /// <summary>
        /// Use this method to delete multiple entities data from database table referring to list of entities passed as parameter.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="message"></param>
        public void Delete(List<T> entities, object message)
        {

        }

        /// <summary>
        /// Use this method to temporary delete single entity data from database table referring to single entity passed as parameter.
        /// This method will do actual delete operation post calling save changes.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool TryToDelete(T entity, object message)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
            return true;
        }
        #endregion

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(GetData(), spec);
        }
        /// <summary>
        /// Use this method to get page wise list of entities by specifying row start index, number of pages, sorting and specification criteria passed within parameters.
        /// And it will also return total items present in the list.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sorting"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        public virtual PaginedList<T> GetListByPage(int startIndex, int pageSize, string sorting, ISpecification<T> spec)
        {
            IQueryable<T> myQuery;
            myQuery = ApplySpecification(spec).AsNoTracking();
            if (!string.IsNullOrEmpty(sorting))
                return new PaginedList<T> { Items = myQuery.OrderBy(sorting).Skip(startIndex).Take(pageSize).ToList(), TotalItems = myQuery.Count() };
            else
                return new PaginedList<T> { Items = myQuery.Skip(startIndex).Take(pageSize).ToList(), TotalItems = myQuery.Count() };
        }

        public static string GetKeyField(Type type)
        {
            PropertyInfo[] allProperties = type.GetProperties();

            PropertyInfo keyProperty = allProperties.SingleOrDefault(p => p.IsDefined(typeof(KeyAttribute)));

            return keyProperty?.Name;
        }
        #region Bulk Actions
        public void BulkInsert(List<T> entities, object message)
        {
            _dbContext.BulkInsert(entities);
            _dbContext.SaveChanges();
        }

        public void BulkUpdate(List<T> entities, object message)
        {
            _dbContext.BulkUpdate(entities);
            _dbContext.SaveChanges();
        }

        public static IQueryable<T> GetData()
        {
            var dataFileName = string.Empty;
            var items = new List<T>();
            dataFileName = "D:\\MoqData\\" + typeof(T).Name + ".json";
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            using (StreamReader r = new StreamReader(dataFileName))
            {
                var json = r.ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    items = System.Text.Json.JsonSerializer.Deserialize<List<T>>(json, options);
                }
            }

            items ??= new List<T>();

            return items.AsQueryable();
        }
        #endregion
    }

    public class MockGenericRepo<T, M> : MockGenericBaseRepo<T, M> where T : class, new() where M : DbContext
    {
        public MockGenericRepo(M dbContext) : base(dbContext)
        {
        }
        /// <summary>
        /// This is virtual method to get list of entities based on search criteria withoout any specification. Use their own Linq to SQL to join multiple tables.
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<T> GetList<S>(S searchModel, object message) where S : class
        {
            return new List<T>();
        }
        /// <summary>
        /// This is virtual method to get single entity based on search criteria withoout any specification. Use their own Linq to SQL to join multiple tables.
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public T GetUniqueRecordBy<S>(S searchModel, object message) where S : class
        {
            return new T();
        }
    }
}
