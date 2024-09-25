using Survey.Data;

namespace Survey.Application.BusinessServices
{
    public class AnswerTableService
    {
        private readonly InboxContext _inboxContext;

        public AnswerTableService(InboxContext inboxContext)
        {
            _inboxContext = inboxContext;
        }
    }
}
