using Microsoft.AspNetCore.Mvc;

namespace Survey.Controllers
{
    [ApiController] // Add this to specify that it's an API controller
    [Route("api/[controller]")] // Route convention for API controllers
    public class FormController : Controller
    {
        [HttpGet("GetForm")]
        public IActionResult Get() 
        {   

            return Ok();
        }
    }
}
