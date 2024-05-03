using Business.Octokit;
using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Services.Static;
using FastSlnPresentation.Server.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastSlnPresentation.Server.Controllers
{
    [Authorize]
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

        /// <summary>
        /// Получить диаграмму классов из репозитория GitHub.
        /// </summary>
        [HttpPost("github")]
        public async Task<IActionResult> GetClassDiagramFromGitRepoAsync(
            [FromBody] GitRepoRequestModel gitRepoRequestModel
        )
        {
            var githubService = new GithubService(gitRepoRequestModel.Pat);

            var allFiles = await githubService.GetAllFiles(
                gitRepoRequestModel.Owner,
                gitRepoRequestModel.RepoName
            );
            var allCodeFiles = allFiles.Where(file => file.Path.EndsWith(".cs")).ToList();

            var graph = _classAnalysisService.AnalyzeCodeFiles(allCodeFiles);

            string json = JsonService.Serialize(graph);

            return Ok(json);
        }

        /// <summary>
        /// Получить диаграмму классов из zip-архива.
        /// </summary>
        [HttpPost("zip-file")]
        public IActionResult GetClassDiagramFromArchiveAsync([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using (var fileStream = file.OpenReadStream())
            {
                var contents = ZipService.ExtractArchiveContents(fileStream);

                var allCodeFiles = contents.Where(file => file.Path.EndsWith(".cs")).ToList();

                var graph = _classAnalysisService.AnalyzeCodeFiles(allCodeFiles);

                var json = JsonService.Serialize(graph);

                return Ok(json);
            }
        }
    }
}
