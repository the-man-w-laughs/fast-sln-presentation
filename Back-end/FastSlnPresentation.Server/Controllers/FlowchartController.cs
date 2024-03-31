using System.Text.Encodings.Web;
using System.Text.Json;
using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Services.Static;
using Microsoft.AspNetCore.Mvc;

namespace FastSlnPresentation.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlowchartController : Controller
    {
        private readonly ILogger<ClassDiagramController> _logger;
        private readonly IMethodAnalysisService _methodAnalysisService;

        public FlowchartController(
            ILogger<ClassDiagramController> logger,
            IMethodAnalysisService methodAnalysisService
        )
        {
            _logger = logger;
            _methodAnalysisService = methodAnalysisService;
        }

        [HttpPost("")]
        public IActionResult GetFlowchart([FromBody] string methodStr)
        {
            var graph = _methodAnalysisService.AnalyzeStringAsync(methodStr);

            var json = JsonService.Serialize(graph);

            return Ok(json);
        }
    }
}
