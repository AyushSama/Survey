using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Survey.Data.DBEntities
{
    public class AnswerTable
    {
        [Key]
        public int answerId { get; set; } // Corresponds to answerId

        [ForeignKey("QuestionTable")]
        public int questionId { get; set; } // Foreign key referencing QuestionTable

        public int answerOption { get; set; } // Corresponds to answerOption

        public string answerDesc { get; set; } // Corresponds to answerDesc
    }
}
