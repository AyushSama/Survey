using Microsoft.AspNetCore.Mvc;
using Survey.Application.BusinessInterfaces;
using Survey.Model;

namespace Survey.Controllers
{
    [ApiController] // Add this to specify that it's an API controller
    [Route("api/")] // Route convention for API controllers
    public class FormController : Controller
    {

        //private readonly IAnswerTableService _answerTableService;
        //private readonly IQuestionTableService _questionTableService;
        private readonly IFormTableService _formTableService;
        private readonly ICopyFormIntoTableProc _copyFormIntoTableProc;
        //IAnswerTableService answerTableService, IQuestionTableService questionTableService,
        public FormController(IFormTableService formTableService, ICopyFormIntoTableProc copyFormIntoTableProc)
        {
            //_answerTableService = answerTableService;
            //_questionTableService = questionTableService;
            _formTableService = formTableService;
            _copyFormIntoTableProc = copyFormIntoTableProc;
        }


        [HttpGet("GetForm")]
        public IActionResult GetForm(int formId)
        {
            var list = _formTableService.getForm(formId);
            return Ok(new { form = list });
        }

        [HttpPost("InsertForm")]
        public IActionResult InsertForm(FormModel form)
        {
            var list = _formTableService.insertForm(form);
            return Ok(new { form = list });
        }


        [HttpGet("ExecuteCopyForm")]
        public IActionResult ExecuteCopyForm(int formId)
        {
            var list = _copyFormIntoTableProc.ExecuteProcedure(formId);
            return Ok(new { list });
        }
    }
}
