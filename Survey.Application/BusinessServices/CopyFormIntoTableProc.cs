using DataHelper.SPData.Common.Repositories;
using Microsoft.SqlServer.Server;
using Survey.Application.BusinessInterfaces;
using Survey.Data.DBEntities;
using Survey.Data.ProcNames;
using Survey.Model;

namespace Survey.Application.BusinessServices
{
    public class CopyFormIntoTableProc : GenericSPRepository<CopyFormIntoTableModel, CopyFormIntoTableProcResult>, ICopyFormIntoTableProc
    {
        public CopyFormIntoTableProc() : base(CopyFormIntoTableProcName.CopyFormIntoTableProcedure) { }

        public List<CopyFormIntoTableModel> ExecuteProcedure(int formId)
        {

            var result = GetSPData(SetSPParameters(formId), new {});
            return result;
        }

        private static object SetSPParameters(int id)
        {
            return new { @formId = id };
        }

        protected override List<CopyFormIntoTableModel> SetEntityResult(object message, IEnumerable<CopyFormIntoTableProcResult> spResult)
        {
            return (from a in spResult
                    select new CopyFormIntoTableModel
                    {
                        FormId = a.formId,
                        AnswerDesc = a.answerDesc,
                        AnswerId = a.answerId,
                        AnswerOption = a.answerOption,
                        CreatedDate = a.createdDate,
                        FormName = a.formName,
                        QuestionDesc = a.questionDesc,
                        QuestionId = a.questionId,
                        QuestionType = a.questionType                        
                    }).ToList();
        }
    }
}
