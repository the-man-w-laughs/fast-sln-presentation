using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Services.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastSlnPresentation.Server.Controllers
{
    [Authorize]
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

        /// <summary>
        /// Получить блок-схему из кода метода.
        /// </summary>
        [HttpPost("")]
        public IActionResult GetFlowchart([FromBody] string methodStr)
        {
            var graph = _methodAnalysisService.AnalyzeStringAsync(methodStr);

            var json = JsonService.Serialize(graph);

            return Ok(json);
        }
    }
}
