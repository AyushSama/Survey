using Survey.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Model
{
    public class QuestionWithAnswerModel
    {
        public QuestionTable Question { get; set; }
        public List<AnswerTable> Answers { get; set; }
    }
}
