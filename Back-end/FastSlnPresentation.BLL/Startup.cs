using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using FastSlnPresentation.BLL.Creators;
using FastSlnPresentation.BLL.Models;
using FastSlnPresentation.BLL.Services;

class FastSlnPresentationMain
{
    // static async Task Main()
    // {
    //     var solutionParser = new SlnParser();
    //     var projectParser = new CsprojParser();

    //     var pat = ConfigurationManager.AppSettings.Get("pat");
    //     var githubService = new GithubService(pat);

    //     var stopwatch = Stopwatch.StartNew();
    //     var allFiles = await githubService.GetAllFiles("the-man-w-laughs", "Obj-Renderer");
    //     Console.WriteLine(stopwatch.ElapsedMilliseconds);
    //     Console.WriteLine(allFiles.Count);

    //     var contentFileService = new ContentFileService(solutionParser, projectParser);

    //     var slnTrees = contentFileService.GetSnlTrees(allFiles);
    // }


    static async Task Main()
    {
        // var solutionParser = new SlnParser();
        // var projectParser = new CsprojParser();

        // var pat = ConfigurationManager.AppSettings.Get("pat");
        // var githubService = new GithubService(pat);

        // var stopwatch = Stopwatch.StartNew();
        // var allFiles = await githubService.GetAllFiles("the-man-w-laughs", "Obj-Renderer");
        // Console.WriteLine(stopwatch.ElapsedMilliseconds);
        // Console.WriteLine(allFiles.Count);
        // var allContent = allFiles.Select(file => file.Content).ToList();

        string directory = "/home/nazar/projects/dotnetResearch/Research";
        string filePath = $"{directory}/Program.cs";
        string content = File.ReadAllText(filePath);
        var allFiles = new List<ContentFile>() { new ContentFile("penis.cs", content) };

        // string directory = "/home/nazar/projects/dotnetResearch";
        // var allFiles = Directory
        //     .GetFiles(directory, "*.cs", SearchOption.AllDirectories)
        //     .Select(
        //         filePath =>
        //             new ContentFile(
        //                 filePath.Substring(directory.Length + 1),
        //                 File.ReadAllText(filePath)
        //             )
        //     )
        //     .ToList();
        // var codeAnalysisService = new CodeAnalysisService(new ClassToXmlWalkerCreator());
        var codeAnalysisService = new CodeAnalysisService(
            new MethodToJsonWalkerCreator(new IdService())
        );

        var graph = codeAnalysisService.AnalyzeCodeFiles(allFiles);
        var serializeOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };
        string json = JsonSerializer.Serialize(graph, serializeOptions);

        string resultPath = $"{directory}/result.json";
        File.WriteAllText(resultPath, json, Encoding.UTF8);
    }
}
