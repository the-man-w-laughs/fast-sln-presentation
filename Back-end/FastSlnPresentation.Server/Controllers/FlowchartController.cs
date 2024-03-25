using Microsoft.AspNetCore.Mvc;

namespace FastSlnPresentation.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlowchartController : Controller
    {
        private readonly ILogger<ClassDiagramController> _logger;

        public FlowchartController(ILogger<ClassDiagramController> logger)
        {
            _logger = logger;
        }

        [HttpPost("")]
        public IActionResult GetFlowchart([FromBody] string methodStr)
        {
            return Ok(methodStr);
        }
    }
}
