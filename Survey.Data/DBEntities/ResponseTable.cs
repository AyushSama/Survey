using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Survey.Data.DBEntities
{
    public class ResponseTable
    {
        [Key]
        public int responseId { get; set; } // Corresponds to responseId

        [ForeignKey("FormTable")]
        public int formId { get; set; } // Foreign key referencing FormTable

        [ForeignKey("QuestionTable")]
        public int questionId { get; set; } // Foreign key referencing QuestionTable

        [ForeignKey("AnswerTable")]
        public int answerId { get; set; } // Foreign key referencing AnswerTable
    }
}
