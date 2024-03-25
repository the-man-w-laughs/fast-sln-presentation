using Microsoft.AspNetCore.Mvc;

namespace FastSlnPresentation.Server.Controllers
{
    [Route("[controller]")]
    public class ClassDiagramController : Controller
    {
        private readonly ILogger<ClassDiagramController> _logger;

        public ClassDiagramController(ILogger<ClassDiagramController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Get class diagrams!");
        }
    }
}
