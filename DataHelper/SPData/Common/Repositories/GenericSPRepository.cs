using DataHelper.Entities.EnumFields;
using DataHelper.SPData.Common.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataHelper.SPData.Common.Repositories
{
    public abstract class GenericSPRepository<M, S> : BaseRepository, IGenericSPRepository<M, S> where M : class, new() where S : class, new()
    {
        #region Constructors
        protected GenericSPRepository() { }
        protected GenericSPRepository(string spName) { _spName = spName; }
        protected GenericSPRepository(string spName, YesNoFlag isTrimRequired) { _spName = spName; _isTrimRequired = isTrimRequired; }
        protected GenericSPRepository(string spName, SPParamType paramType, int structuredParamIndex) { _spName = spName; _spParamType = paramType; _structuredParamIndex = structuredParamIndex; }
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
            var paramArr = SetParams(inputparameters);
            List<S> Tempresult = ExtraSqlQueryWithDataReader<S>(message, paramArr);
            return SetEntityResult(message, Tempresult);
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
            var paramArr = SetParams(inputparameters, paramType, false);
            List<S> Tempresult = ExtraSqlQueryWithDataReader<S>(message, paramArr);
            return SetEntityResult(message, Tempresult);
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
            var paramArr = SetParams(inputparameters, IsOutParam);
            var Tempresult = ExtraSqlQueryWithDataReaderWithDataSet<S>(message, paramArr);
            return Tempresult;
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
            var paramArr = SetParams(inputparameters, paramType, false);
            try
            {
                _ = ExecuteNonQuerySP(message, paramArr);
            }
            catch
            {
                throw;
            }
            return new S();
        }
        /// <summary>
        /// Use this method to insert / update / delete data referring to SQL query passed as parameter. This method does not return any object or data.
        /// </summary>
        /// <param name="inlineSQL"></param>
        /// <param name="message"></param>
        public void SaveUpdateDeleteInlineQuery(string inlineSQL, object message)
        {
            _ = ExecuteNonQueryInlineSQL(inlineSQL, message);
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
            using SqlConnection con = new(GetConnectionString());
            using SqlCommand cmd = new(_spName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = (int)TimeoutValues.Unlimited;
            foreach (var param in parameters)
                cmd.Parameters.Add(param);
            SqlDataAdapter adpt = new(cmd);
            adpt.Fill(ds);
            return ds;
        }
        protected virtual DataSet ExtraSqlQueryWithDataReaderWithDataSet<T>(string inlineSQL, object message)
        {
            DataSet ds = new();
            using SqlConnection con = new(GetConnectionString());
            using SqlCommand cmd = new(inlineSQL, con);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = (int)TimeoutValues.Unlimited;
            SqlDataAdapter adpt = new(cmd);
            adpt.Fill(ds);
            return ds;
        }
        protected virtual int ExecuteNonQuerySP(object message, params object[] parameters)
        {
            int Tempresult;
            using SqlConnection con = new(GetConnectionString());
            con.Open();
            using SqlCommand cmd = new(_spName, con);
            System.Text.StringBuilder sb = new();
            cmd.CommandTimeout = (int)TimeoutValues.Unlimited;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var param in parameters)
                cmd.Parameters.Add(param);

            foreach (SqlParameter par in cmd.Parameters)
            {
                try
                {
                    sb.AppendLine("@" + par.ParameterName.ToString().Trim() + " = '" + Convert.ToString("" + par.Value).Trim() + "'");
                }
                catch
                {
                    //Override any unhandled exception here
                }
            }
            Tempresult = cmd.ExecuteNonQuery();
            return Tempresult;
        }
        protected virtual int ExecuteNonQueryInlineSQL(string inlineSQL, object message)
        {
            int Tempresult;
            using SqlConnection con = new(GetConnectionString());
            con.Open();
            using SqlCommand cmd = new(inlineSQL, con);
            System.Text.StringBuilder sb = new();
            cmd.CommandTimeout = (int)TimeoutValues.Unlimited;
            cmd.CommandType = CommandType.Text;

            Tempresult = cmd.ExecuteNonQuery();
            return Tempresult;
        }
        protected virtual S SetUpdatedEntity(object inputparameters)
        {
            return new S();
        }
        #endregion
    }
}
