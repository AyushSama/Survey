using System.ComponentModel.DataAnnotations;

namespace Survey.Data.DBEntities
{
    public class FormTable
    {
        [Key]
        public int formId { get; set; } // Corresponds to formId
        public string formName { get; set; } // Corresponds to formName
        public DateTime createdDate { get; set; } // Corresponds to createdDate

        // Constructor to initialize CreatedDate with the current date
        public FormTable()
        {
            createdDate = DateTime.Now;
        }
    }
}
