using DataHelper.Entities.EnumFields;
using DataHelper.HelperClasses;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Text;

namespace DataHelper.SPData.Common.Repositories
{
    public class BaseRepository
    {
        protected StringBuilder _parametersSeq;
        protected string _spName;
        protected YesNoFlag _isTrimRequired = YesNoFlag.Yes;
        protected int? _outParamIndex;
        protected int? _structuredParamIndex;
        internal SPParamType _spParamType;
        public BaseRepository()
        {
            _parametersSeq = new StringBuilder();
        }

        private static IDictionary<string, object> ToDictionary(object data)
        {
            var attr = BindingFlags.Public | BindingFlags.Instance;
            var dict = new Dictionary<string, object>();
            foreach (var property in data.GetType().GetProperties(attr))
            {
                if (property.CanRead)
                {
                    dict.Add(property.Name, property.GetValue(data, null));
                }
            }
            return dict;
        }
        protected object[] SetParams(object inputparameters)
        {
            var inputparams = ToDictionary(inputparameters);
            int i = 0;
            var arr = new object[inputparams.Count];
            _parametersSeq.Clear();
            {
                foreach (var one_kvp in inputparams)
                {
                    var param = new SqlParameter
                    {
                        ParameterName = one_kvp.Key,
                        Value = one_kvp.Value ?? DBNull.Value
                    };
                    arr[i] = param;
                    _parametersSeq.Append(" @" + one_kvp.Key + ",");
                    i++;
                }
                if (_parametersSeq.Length > 0)
                    _parametersSeq.Remove(_parametersSeq.Length - 1, 1);
            }
            return arr;
        }
        protected object[] SetParams(object inputparameters, object outputparameters)
        {
            var inputparams = ToDictionary(inputparameters);
            var outputparams = ToDictionary(outputparameters);
            int i = 0;
            var arr = new object[inputparams.Count + outputparams.Count];
            _parametersSeq.Clear();
            {
                foreach (var one_kvp in inputparams)
                {
                    var param = new SqlParameter
                    {
                        ParameterName = one_kvp.Key,
                        Value = one_kvp.Value ?? DBNull.Value
                    };
                    arr[i] = param;
                    _parametersSeq.Append(" @" + one_kvp.Key + ",");
                    i++;
                }
                foreach (var one_kvp in outputparams)
                {
                    var param = new SqlParameter
                    {
                        ParameterName = one_kvp.Key,
                        Value = one_kvp.Value ?? DBNull.Value,
                        Direction = ParameterDirection.Output
                    };
                    param.Size = param.DbType == DbType.String ? 2000 : param.Size;
                    arr[i] = param;
                    _parametersSeq.Append(" @" + one_kvp.Key + ",");
                    i++;
                }
                if (_parametersSeq.Length > 0)
                    _parametersSeq.Remove(_parametersSeq.Length - 1, 1);
            }
            return arr;
        }
        protected object[] SetParams(object inputparameters, bool IsOutParam)
        {
            var inputparams = ToDictionary(inputparameters);
            int i = 0;
            var arr = IsOutParam ? new object[inputparams.Count + 2] : new object[inputparams.Count];
            _parametersSeq.Clear();
            {
                foreach (var one_kvp in inputparams)
                {
                    var param = new SqlParameter
                    {
                        ParameterName = one_kvp.Key,
                        Value = one_kvp.Value ?? DBNull.Value
                    };
                    arr[i] = param;
                    _parametersSeq.Append(" @" + one_kvp.Key + ",");
                    i++;

                    if (IsOutParam && _outParamIndex != null && i == _outParamIndex)
                    {
                        AddOutParam(arr, i);
                        i++;
                        i++;
                        _parametersSeq.Append(',');
                    }
                }
                if (!IsOutParam || _outParamIndex != null)
                {
                    if (_parametersSeq.Length > 0)
                        _parametersSeq.Remove(_parametersSeq.Length - 1, 1);
                }
                else
                {
                    AddOutParam(arr, i);
                }
            }
            return arr;
        }
        protected object[] SetParams(object inputparameters, SPParamType paramtype, bool IsOutParam = false)
        {
            IDictionary<string, object> inputparams = ToDictionary(inputparameters);
            int i = 0;
            var arr = IsOutParam ? new object[inputparams.Count + 2] : new object[inputparams.Count];
            _parametersSeq.Clear();
            {
                foreach (var one_kvp in inputparams)
                {
                    var param = new SqlParameter
                    {
                        ParameterName = one_kvp.Key,
                        Value = one_kvp.Value ?? DBNull.Value
                    };
                    if (i == _structuredParamIndex && paramtype == SPParamType.Structured)
                    {
                        param.SqlDbType = SqlDbType.Structured;
                        param.Direction = ParameterDirection.Input;
                    }
                    arr[i] = param;
                    _parametersSeq.Append(" @" + one_kvp.Key + ",");
                    i++;

                    if (IsOutParam && _outParamIndex != null && i == _outParamIndex)
                    {
                        AddOutParam(arr, i);
                        i++;
                        i++;
                        _parametersSeq.Append(',');
                    }
                }
                if (IsOutParam && _outParamIndex == null)
                {
                    AddOutParam(arr, i);
                }
                else
                {
                    if (_parametersSeq.Length > 0)
                        _parametersSeq.Remove(_parametersSeq.Length - 1, 1);
                }
            }
            return arr;
        }
        private void AddOutParam(object[] arr, int startIndex)
        {

            var outparam = new SqlParameter
            {
                ParameterName = "PStatus",
                Direction = ParameterDirection.Output,
                DbType = DbType.Int32
            };

            arr[startIndex] = outparam;
            _parametersSeq.Append(" @PStatus OUT,");

            outparam = new SqlParameter
            {
                ParameterName = "PMsg",
                Direction = ParameterDirection.Output,
                DbType = DbType.String,
                Size = 60
            };
            arr[startIndex + 1] = outparam;
            _parametersSeq.Append(" @PMsg OUT");
        }

        #region Connection Related    
        public static string GetConnectionString()
        {
            return UserConfiguration.ConnectionBuilder;
        }
        protected List<T> ExtraSqlQueryWithDataReader<T>(object message, params object[] parameters)
        {
            List<T> Rows = new();
            try
            {
                using SqlConnection con = new(GetConnectionString());
                using SqlCommand cmd = new(_spName, con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                using SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    int noRecCount = 0;
                    int i = 0;
                    List<ResultEntityColumnMapping> lstResultSetColMap = new();
                    var lstClassProperties = typeof(T).GetProperties().ToList();
                    foreach (DataRow colDetail in dr.GetSchemaTable().Rows)
                    {
                        ResultEntityColumnMapping col = new()
                        {
                            SPColumnName = colDetail["ColumnName"].ToString(),
                            SPColumnIndex = i
                        };

                        if (col.SPColumnName == "")
                        {
                            col.ClassPropertyName = "No_column_name" + (++noRecCount).ToString();
                        }
                        else
                        {
                            if (Convert.ToString("" + col.SPColumnName).ToLower() != "default")
                            {
                                col.ClassPropertyName = Convert.ToString("" + col.SPColumnName).Trim().Replace(" ", "_").Replace(".", "_").Replace("(", "_").Replace(")", "_");
                                string lColName = lstClassProperties.Where(p => p.Name.ToLower() == col.ClassPropertyName.ToLower()).Select(q => q.Name).FirstOrDefault();
                                if (!string.IsNullOrEmpty(lColName))
                                    col.ClassPropertyName = lColName;
                            }
                            else
                                col.ClassPropertyName = col.SPColumnName + "1";
                        }
                        lstResultSetColMap.Add(col);
                        i++;
                    }

                    while (dr.Read())
                    {
                        T tempObj = (T)Activator.CreateInstance(typeof(T));
                        foreach (ResultEntityColumnMapping col in lstResultSetColMap)
                        {
                            try
                            {
                                PropertyInfo propertyInfo = tempObj.GetType().GetProperty(col.ClassPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                if (null != propertyInfo && propertyInfo.CanWrite)
                                    propertyInfo.SetValue(tempObj, ConvertToPropType(propertyInfo, dr[col.SPColumnIndex]), null);
                            }
                            catch
                            {
                                message = "Fail";
                            }
                        }
                        Rows.Add(tempObj);
                    }
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                message = "Fail";
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                message = "Fail";
                ex = ex.InnerException ?? ex;
                throw;
            }
            return Rows;
        }
        protected List<T> ExtraSqlQueryWithDataReader<T>(string InlineQuery, object message)
        {
            List<T> Rows = new();
            try
            {
                using SqlConnection con = new(GetConnectionString());
                using SqlCommand cmd = new(InlineQuery, con);
                con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;

                using SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    int noRecCount = 0;
                    int i = 0;
                    List<ResultEntityColumnMapping> lstResultSetColMap = new();
                    var lstClassProperties = typeof(T).GetProperties().ToList();
                    foreach (DataRow colDetail in dr.GetSchemaTable().Rows)
                    {
                        ResultEntityColumnMapping col = new()
                        {
                            SPColumnName = colDetail["ColumnName"].ToString(),
                            SPColumnIndex = i
                        };

                        if (col.SPColumnName == "")
                        {
                            col.ClassPropertyName = "No_column_name" + (++noRecCount).ToString();
                        }
                        else
                        {
                            if (Convert.ToString("" + col.SPColumnName).ToLower() != "default")
                            {
                                col.ClassPropertyName = Convert.ToString("" + col.SPColumnName).Trim().Replace(" ", "_").Replace(".", "_").Replace("(", "_").Replace(")", "_");
                                string lColName = lstClassProperties.Where(p => p.Name.ToLower() == col.ClassPropertyName.ToLower()).Select(q => q.Name).FirstOrDefault();
                                if (!string.IsNullOrEmpty(lColName))
                                    col.ClassPropertyName = lColName;
                            }
                            else
                                col.ClassPropertyName = col.SPColumnName + "1";
                        }
                        lstResultSetColMap.Add(col);
                        i++;
                    }

                    while (dr.Read())
                    {
                        T tempObj = (T)Activator.CreateInstance(typeof(T));
                        foreach (ResultEntityColumnMapping col in lstResultSetColMap)
                        {
                            try
                            {
                                PropertyInfo propertyInfo = tempObj.GetType().GetProperty(col.ClassPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                if (null != propertyInfo && propertyInfo.CanWrite)
                                    propertyInfo.SetValue(tempObj, ConvertToPropType(propertyInfo, dr[col.SPColumnIndex]), null);
                            }
                            catch
                            {
                                message = "Fail";
                            }
                        }
                        Rows.Add(tempObj);
                    }
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                message = "Fail";
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                message = "Fail";
                ex = ex.InnerException ?? ex;
                throw;
            }
            return Rows;
        }
        protected object ConvertToPropType(PropertyInfo property, object value)
        {
            object cstVal = null;
            if (property != null)
            {
                Type propType = Nullable.GetUnderlyingType(property.PropertyType);
                bool isNullable = propType != null;
                if (!isNullable) { propType = property.PropertyType; }
                bool canAttrib = value != null || isNullable;
                if (!canAttrib) { throw new Exception("Can not add attribute null on non nullable. "); }
                cstVal = value == null || Convert.IsDBNull(value) ? null : Convert.ChangeType(value, propType ?? property.PropertyType);
                if (cstVal != null && cstVal.GetType() == typeof(string) && _isTrimRequired == YesNoFlag.Yes)
                    cstVal = ((string)cstVal).Trim();
                else if (cstVal != null && cstVal.GetType() == typeof(string) && _isTrimRequired == YesNoFlag.No)
                    cstVal = (string)cstVal;
            }
            return cstVal;
        }
        #endregion
    }

    public class ResultEntityColumnMapping
    {
        public string SPColumnName { get; set; }
        public int SPColumnIndex { get; set; }
        public string ClassPropertyName { get; set; }
    }
}
