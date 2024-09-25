using Survey.Data;

namespace Survey.Application.BusinessServices
{
    public class QuestionTableService
    {
        private readonly InboxContext _inboxContext;

        public QuestionTableService(InboxContext inboxContext)
        {
            _inboxContext = inboxContext;
        }
    }
}
