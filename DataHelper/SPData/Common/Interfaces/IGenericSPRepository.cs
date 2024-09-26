using DataHelper.Entities.EnumFields;
using System.Data;

namespace DataHelper.SPData.Common.Interfaces
{
    public interface IGenericSPRepository<M, S> where M : class, new() where S : class, new()
    {
        List<M> GetSPData(object inputparameters, object message);
        List<M> GetSPData(object inputparameters, SPParamType paramType, object message);
        DataSet GetSPDataSet(object inputparameters, bool IsOutParam, object message);
        void SaveUpdateDeleteSPData(object inputparameters, object message);
        S SaveUpdateDeleteSPData(object inputparameters, object message, YesNoFlag ReturnEntity);
        S SaveUpdateDeleteSPData(object inputparameters, SPParamType paramType, object message);
        void SaveUpdateDeleteInlineQuery(string inlineSQL, object message);
        List<M> GetInlineQueryData(string inlineSQL, object message);
        DataSet GetInlineQueryDataSet(string inlineSQL, object message);
    }
}
