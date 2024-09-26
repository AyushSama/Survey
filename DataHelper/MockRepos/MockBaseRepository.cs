using DataHelper.Entities.EnumFields;
using System.Reflection;
using System.Text;

namespace DataHelper.MockRepos
{
    public class MockBaseRepository
    {
        protected StringBuilder _parametersSeq;
        protected string _spName;
        protected YesNoFlag _isTrimRequired = YesNoFlag.Yes;
        protected int? _outParamIndex;
        protected int? _structuredParamIndex;
        protected SPParamType _spParamType;
        public MockBaseRepository()
        {
            _parametersSeq = new StringBuilder();
        }

        private static IDictionary<string, object> ToDictionary(object data)
        {
            var dict = new Dictionary<string, object>();
            return dict;
        }
        protected object[] SetParams(object inputparameters)
        {
            var inputparams = ToDictionary(inputparameters);
            var arr = new object[inputparams.Count];
            return arr;
        }
        protected object[] SetParams(object inputparameters, object outputparameters)
        {
            var inputparams = ToDictionary(inputparameters);
            var outputparams = ToDictionary(outputparameters);
            var arr = new object[inputparams.Count + outputparams.Count];
            return arr;
        }
        protected object[] SetParams(object inputparameters, bool IsOutParam)
        {
            var inputparams = ToDictionary(inputparameters);
            var arr = IsOutParam ? new object[inputparams.Count + 2] : new object[inputparams.Count];
            return arr;
        }
        protected object[] SetParams(object inputparameters, SPParamType paramtype, bool IsOutParam = false)
        {
            IDictionary<string, object> inputparams = ToDictionary(inputparameters);
            var arr = IsOutParam ? new object[inputparams.Count + 2] : new object[inputparams.Count];
            return arr;
        }
        private void AddOutParam(object[] arr, int startIndex)
        {
        }

        #region Connection Related    
        public static string GetConnectionString()
        {
            return string.Empty;
        }
        protected List<T> ExtraSqlQueryWithDataReader<T>(object message, params object[] parameters)
        {
            List<T> Rows = new();
            return Rows;
        }
        protected List<T> ExtraSqlQueryWithDataReader<T>(string InlineQuery, object message)
        {
            List<T> Rows = new();
            return Rows;
        }
        protected object ConvertToPropType(PropertyInfo property, object value)
        {
            object cstVal = null;
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
