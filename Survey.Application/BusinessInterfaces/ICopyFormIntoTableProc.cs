using DataHelper.SPData.Common.Interfaces;
using Survey.Data.DBEntities;
using Survey.Model;

namespace Survey.Application.BusinessInterfaces
{
    public interface ICopyFormIntoTableProc : IGenericSPRepository<CopyFormIntoTableModel,CopyFormIntoTableProcResult>
    {
        List<CopyFormIntoTableModel> ExecuteProcedure(int formId);
    }
}
