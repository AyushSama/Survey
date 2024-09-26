namespace Survey.Model
{
    public class CopyFormIntoTableModel
    {
        public int FormId { get; set; }
        public string FormName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int QuestionId { get; set; }
        public string QuestionDesc { get; set; }
        public string QuestionType { get; set; }
        public int AnswerId { get; set; }
        public int AnswerOption { get; set; }
        public string AnswerDesc { get; set; }

    }
}
