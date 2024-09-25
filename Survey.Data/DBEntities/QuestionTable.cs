using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Survey.Data.DBEntities
{
    public class QuestionTable
    {
        [Key]
        public int questionId { get; set; } // Corresponds to questionId
        public string questionDesc { get; set; } // Corresponds to questionDesc
        public string questionType { get; set; } // Corresponds to questionType

        [ForeignKey("FormTable")]
        public int formId { get; set; } // Foreign key referencing FormTable

    }
}
