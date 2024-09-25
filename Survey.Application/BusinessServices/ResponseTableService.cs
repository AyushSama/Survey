using Survey.Data;

namespace Survey.Application.BusinessServices
{
    public class ResponseTableService
    {
        private readonly InboxContext _inboxContext;

        public ResponseTableService(InboxContext inboxContext)
        {
            _inboxContext = inboxContext;
        }
    }
}
