using Microsoft.EntityFrameworkCore;
using Survey.Data.DBEntities;
using System.Collections.Generic;

namespace Survey.Data
{
    public class InboxContext(DbContextOptions<InboxContext> options) : DbContext(options)
    {
        public DbSet<FormTable> FormTable { get; set; }
        public DbSet<QuestionTable> QuestionTable { get; set; }
        public DbSet<AnswerTable> AnswerTable { get; set; }
        public DbSet<ResponseTable> ResponseTable { get; set; }

    }
}
