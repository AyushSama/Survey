using DataHelper.Entities.EnumFields;
using DataHelper.SPData.Common.Interfaces;
using System.Data;
using System.Text.Json;

namespace DataHelper.MockRepos
{
    public abstract class MockGenericSPRepository<M, S> : MockBaseRepository, IGenericSPRepository<M, S> where M : class, new() where S : class, new()
    {
        #region Constructors
        protected MockGenericSPRepository() { }
        protected MockGenericSPRepository(string spName) { _spName = spName; }
        protected MockGenericSPRepository(string spName, YesNoFlag isTrimRequired) { _spName = spName; _isTrimRequired = isTrimRequired; }
        protected MockGenericSPRepository(string spName, SPParamType paramType, int structuredParamIndex) { _spName = spName; _spParamType = paramType; _structuredParamIndex = structuredParamIndex; }
        #endregion

        #region public methods
        /// <summary>
        /// Use this method to get multiple entities data referring to stored procedure passed as parameter along with procedure parameters.
        /// You must override SetEntityResult to map sp result into model entity either using Automapper or Linq.
        /// </summary>
        /// <param name="inputparameters"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<M> GetSPData(object inputparameters, object message)
        {
            return GetMockSPData().ToList();
        }
        /// <summary>
        /// Use this method to get multiple entities data referring to stored procedure passed as parameter along with parameter type and procedure parameters.
        /// When user wants to pass table type as parameter parameter and call the stored procedure, use "SPParamType.Structured" in paramType.
        /// You must override SetEntityResult to map sp result into model entity either using Automapper or Linq.
        /// </summary>
        /// <param name="inputparameters"></param>
        /// <param name="paramType"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<M> GetSPData(object inputparameters, SPParamType paramType, object message)
        {
            return GetMockSPData().ToList();
        }
        /// <summary>
        /// Use this method to get data using dataset referring to stored procedure passed as parameter along with procedure parameters and out parameter list.
        /// When user wants to get data in stored procedure's out parameter, pass IsOutParam = true.
        /// </summary>
        /// <param name="inputparameters"></param>
        /// <param name="IsOutParam"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public DataSet GetSPDataSet(object inputparameters, bool IsOutParam, object message)
        {
            return GetMockSPDataSet();
        }

        public DataSet GetMockSPDataSet()
        {
            var result = GetMockSPData().ToList();
            //convert list data to dataset
            return new DataSet();
        }

        public DataSet GetInlineQueryDataSet(string inlineSQL, object message)
        {
            var Tempresult = ExtraSqlQueryWithDataReaderWithDataSet<S>(inlineSQL, message);
            return Tempresult;
        }
        /// <summary>
        /// Use this method to insert / update / delete data referring to stored procedure passed as parameter along with procedure parameters.
        /// This method does not return any object or data.
        /// </summary>
        /// <param name="inputparameters"></param>
        /// <param name="message"></param>
        public void SaveUpdateDeleteSPData(object inputparameters, object message)
        {
            var paramArr = SetParams(inputparameters);
            _ = ExecuteNonQuerySP(message, paramArr);
        }
        /// <summary>
        /// Use this method to insert / update / delete data referring to stored procedure passed as parameter along with procedure parameters.
        /// This method returns object of type referring to calling function type.
        /// You must override SetUpdateEntity to get the entity return mapped from input parameter.
        /// </summary>
        /// <param name="inputparameters"></param>
        /// <param name="message"></param>
        /// <param name="ReturnEntity"></param>
        /// <returns></returns>
        public S SaveUpdateDeleteSPData(object inputparameters, object message, YesNoFlag ReturnEntity)
        {
            var paramArr = SetParams(inputparameters);
            _ = ExecuteNonQuerySP(message, paramArr);
            return SetUpdatedEntity(inputparameters);
        }
        /// <summary>
        /// Use this method to insert / update / delete data referring to stored procedure passed as parameter along with parameter type and procedure parameters.
        /// When user wants to pass table type as parameter and call the stored procedure, use "SPParamType.Structured" in paramType.
        /// </summary>
        /// <param name="inputparameters"></param>
        /// <param name="paramType"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public S SaveUpdateDeleteSPData(object inputparameters, SPParamType paramType, object message)
        {
            return new S();
        }
        /// <summary>
        /// Use this method to insert / update / delete data referring to SQL query passed as parameter. This method does not return any object or data.
        /// </summary>
        /// <param name="inlineSQL"></param>
        /// <param name="message"></param>
        public void SaveUpdateDeleteInlineQuery(string inlineSQL, object message)
        {

        }
        /// <summary>
        /// Use this method to get multiple entities referring to SQL query passed as parameter. This method returns list of entities.
        /// </summary>
        /// <param name="inlineSQL"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<M> GetInlineQueryData(string inlineSQL, object message)
        {
            List<S> Tempresult = ExtraSqlQueryWithDataReader<S>(inlineSQL, message);
            return SetEntityResult(message, Tempresult);
        }
        #endregion

        #region virtual methods
        protected virtual List<M> SetEntityResult(object message, IEnumerable<S> spResult)
        {
            return new List<M>();
        }

        protected virtual DataSet ExtraSqlQueryWithDataReaderWithDataSet<T>(object message, params object[] parameters)
        {
            DataSet ds = new();
            return ds;
        }
        protected virtual DataSet ExtraSqlQueryWithDataReaderWithDataSet<T>(string inlineSQL, object message)
        {
            DataSet ds = new();
            return ds;
        }
        protected virtual int ExecuteNonQuerySP(object message, params object[] parameters)
        {
            return 1;
        }
        protected virtual int ExecuteNonQueryInlineSQL(string inlineSQL, object message)
        {
            return 1;
        }

        public static IQueryable<M> GetMockSPData()
        {
            var dataFileName = string.Empty;
            var items = new List<M>();
            dataFileName = "D:\\MoqData\\" + typeof(M).Name + ".json";
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            using (StreamReader r = new StreamReader(dataFileName))
            {
                var json = r.ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    items = System.Text.Json.JsonSerializer.Deserialize<List<M>>(json, options);
                }
            }

            items ??= new List<M>();
            return items.AsQueryable();
        }
        protected virtual S SetUpdatedEntity(object inputparameters)
        {
            return new S();
        }
        #endregion
    }
}
