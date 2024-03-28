using System.Text.Encodings.Web;
using System.Text.Json;
using Business.Octokit;
using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace FastSlnPresentation.Server.Controllers
{
    [Route("[controller]")]
    public class ClassDiagramController : Controller
    {
        private readonly ILogger<ClassDiagramController> _logger;
        private readonly IClassAnalysisService _classAnalysisService;

        public ClassDiagramController(
            ILogger<ClassDiagramController> logger,
            IClassAnalysisService classAnalysisService
        )
        {
            _logger = logger;
            _classAnalysisService = classAnalysisService;
        }

        [HttpPost("")]
        public async Task<IActionResult> GetClassDiagramAsync(
            [FromBody] string pat,
            [FromBody] string owner,
            [FromBody] string repoName
        )
        {
            var githubService = new GithubService(pat);
            var allFiles = await githubService.GetAllFiles(owner, repoName);
            var allCodeFiles = allFiles.Where(file => file.Path.EndsWith(".cs")).ToList();

            var graph = _classAnalysisService.AnalyzeCodeFiles(allCodeFiles);

            var serializeOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };

            var json = JsonSerializer.Serialize(graph, serializeOptions);

            return Ok(json);
        }
    }
}
