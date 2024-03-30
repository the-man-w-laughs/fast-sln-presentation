using System.Text.Encodings.Web;
using System.Text.Json;
using Business.Octokit;
using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Models;
using FastSlnPresentation.BLL.Services.Static;
using FastSlnPresentation.Server.Dtos;
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

            var serializeOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };

            var json = JsonSerializer.Serialize(graph, serializeOptions);

            return Ok(json);
        }

        [HttpPost("zip-file")]
        public IActionResult GetClassDiagramFromArchiveAsync([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using (var fileStream = file.OpenReadStream())
                {
                    var contents = ZipService.ExtractArchiveContents(fileStream);

                    var allCodeFiles = contents.Where(file => file.Path.EndsWith(".cs")).ToList();

                    var graph = _classAnalysisService.AnalyzeCodeFiles(allCodeFiles);

                    var serializeOptions = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    };

                    var json = JsonSerializer.Serialize(graph, serializeOptions);

                    return Ok(json);
                }
            }
            catch (InvalidDataException ex)
            {
                return BadRequest("Invalid file format.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
