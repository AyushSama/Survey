using Survey.Data.DBEntities;

namespace Survey.Model
{
    public class QuestionWithAnswerModel
    {
        public QuestionTable Question { get; set; }
        public List<AnswerTable> Answers { get; set; }
    }
}
