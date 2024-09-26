using Survey.Data.DBEntities;

namespace Survey.Model
{
    public class FormModel
    {
        public FormTable Form { get; set; }
        public List<QuestionWithAnswerModel> QuestionsWithAnswers { get; set; }
    }
}
