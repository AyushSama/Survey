using Survey.Data;

namespace Survey.Application.BusinessServices
{
    public class FormTableService
    {
        private readonly InboxContext _inboxContext;

        public FormTableService(InboxContext inboxContext)
        {
            _inboxContext = inboxContext;
        }
    }
}
