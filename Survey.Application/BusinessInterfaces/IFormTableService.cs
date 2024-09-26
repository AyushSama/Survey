using Survey.Model;

namespace Survey.Application.BusinessInterfaces
{
    public interface IFormTableService
    {
        public FormModel getForm(int formId);
    }
}
