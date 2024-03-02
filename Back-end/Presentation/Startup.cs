using System.Text;
using Newtonsoft.Json;
using Presentation.Creators;
using Presentation.Models;
using Presentation.Models.JsonModels;
using Presentation.Services;

class PresentationMain
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

        // string directory = "/home/nazar/projects/dotnetResearch/Research";
        // string filePath = $"{directory}/Program.cs";
        // string content = File.ReadAllText(filePath);
        // var allFiles = new List<ContentFile>() { new ContentFile("penis.cs", content) };

        string directory = "/home/nazar/projects/dotnetResearch";
        var allFiles = Directory
            .GetFiles(directory, "*.cs", SearchOption.AllDirectories)
            .Select(
                filePath =>
                    new ContentFile(
                        filePath.Substring(directory.Length + 1),
                        File.ReadAllText(filePath)
                    )
            )
            .ToList();
        // var codeAnalysisService = new CodeAnalysisService(new ClassToXmlWalkerCreator());
        var codeAnalysisService = new CodeAnalysisService(new ClassToJsonWalkerCreator());
        var resultXml = codeAnalysisService.AnalyzeCodeFiles(allFiles);

        string resultPath = $"{directory}/result.xml";
        resultXml.Save(resultPath);

        JsonTest();
    }

    static void JsonTest()
    {
        // Create instances of JsonClass
        List<object> jsonClasses = new List<object>
        {
            // new JsonStruct("ClassName2", default, "internal")
        };

        string json = JsonConvert.SerializeObject(jsonClasses);

        string directory = "/home/nazar/projects/dotnetResearch/Research";

        string resultPath = $"{directory}/result.json";
        File.WriteAllText(resultPath, json, Encoding.UTF8);

        Console.WriteLine("JSON data has been written to file: " + resultPath);
    }
}
