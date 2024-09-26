using Survey.Application.BusinessInterfaces;
using Survey.Data;
using Survey.Data.DBEntities;
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

        public FormModel insertForm(FormModel formModel)
        {
            using (var transaction = _inboxContext.Database.BeginTransaction())
            {
                try
                {
                    // Insert Form
                    var formEntity = new FormTable
                    {
                        formName = formModel.Form.formName,
                        createdDate = DateTime.Now
                    };
                    _inboxContext.FormTable.Add(formEntity);
                    _inboxContext.SaveChanges();

                    // Insert Questions and Answers
                    foreach (var questionWithAnswers in formModel.QuestionsWithAnswers)
                    {
                        var questionEntity = new QuestionTable
                        {
                            questionDesc = questionWithAnswers.Question.questionDesc,
                            questionType = questionWithAnswers.Question.questionType,
                            formId = formEntity.formId  // Link to the inserted Form
                        };
                        _inboxContext.QuestionTable.Add(questionEntity);
                        _inboxContext.SaveChanges();

                        // Insert Answers
                        foreach (var answer in questionWithAnswers.Answers)
                        {
                            var answerEntity = new AnswerTable
                            {
                                questionId = questionEntity.questionId,  // Link to the inserted Question
                                answerOption = answer.answerOption,
                                answerDesc = answer.answerDesc
                            };
                            _inboxContext.AnswerTable.Add(answerEntity);
                        }
                        _inboxContext.SaveChanges();
                    }

                    // Commit transaction
                    transaction.Commit();

                    // Return the inserted form with questions and answers
                    return formModel;
                }
                catch (Exception ex)
                {
                    // Rollback transaction if something went wrong
                    transaction.Rollback();
                    throw new Exception("An error occurred while inserting the form: " + ex.Message);
                }
            }
        }

    }
}
