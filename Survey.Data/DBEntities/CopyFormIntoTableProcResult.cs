namespace Survey.Data.DBEntities
{
    public class CopyFormIntoTableProcResult
    {
        public int formId { get; set; }
        public string formName { get; set; }
        public DateTime createdDate { get; set; }
        public int questionId { get; set; }
        public string questionDesc { get; set; }
        public string questionType { get; set; }
        public int answerId { get; set; }
        public int answerOption { get; set; }
        public string answerDesc { get; set; }
    }
}
