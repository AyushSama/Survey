using Survey.Application.BusinessInterfaces;
using Survey.Data;
using Survey.Model;

namespace Survey.Application.BusinessServices
{
    public class FormTableService : IFormTableService
    {
        private readonly InboxContext _inboxContext;

        public FormTableService(InboxContext inboxContext)
        {
            _inboxContext = inboxContext;
        }

        public FormModel getForm(int formId)
        {
            var result = (from form in _inboxContext.FormTable
                          where form.formId == formId
                          select new FormModel
                          {
                              Form = form,
                              QuestionsWithAnswers = (from question in _inboxContext.QuestionTable
                                                      where question.formId == formId
                                                      select new QuestionWithAnswerModel
                                                      {
                                                          Question = question,
                                                          Answers = (from answer in _inboxContext.AnswerTable
                                                                     where answer.questionId == question.questionId
                                                                     select answer).ToList()
                                                      }).ToList()
                          }).FirstOrDefault();

            return result;
        }
    }
}
