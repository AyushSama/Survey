using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Survey.Application.BusinessInterfaces;

namespace Survey.Controllers
{
    [ApiController] // Add this to specify that it's an API controller
    [Route("api/[controller]")] // Route convention for API controllers
    public class FormController : Controller
    {

        //private readonly IAnswerTableService _answerTableService;
        //private readonly IQuestionTableService _questionTableService;
        private readonly IFormTableService _formTableService;
        //IAnswerTableService answerTableService, IQuestionTableService questionTableService,
        public FormController( IFormTableService formTableService)
        {
            //_answerTableService = answerTableService;
            //_questionTableService = questionTableService;
            _formTableService = formTableService;
        }


        [HttpGet("GetForm")]
        public IActionResult Get(int formId) 
        {
            var list = _formTableService.getForm(formId);
            return Ok(new {form = list});
        }
    }
}
