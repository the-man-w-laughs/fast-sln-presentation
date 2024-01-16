using System.Configuration;
using System.Diagnostics;
using System.Xml;
using Business.Octokit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Presentation.Models;
using Presentation.Services;
using Presentation.SyntaxWalkers;

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

        string directory = "/home/nazar/projects/dotnetResearch/Research";
        string filePath = $"{directory}/Program.cs";
        string content = File.ReadAllText(filePath);
        var allFiles = new List<ContentFile>() { new ContentFile("penis.cs", content) };
        var codeAnalysisService = new CodeAnalysisService();
        var resultXml = codeAnalysisService.AnalyzeCodeFiles(allFiles);

        string resultPath = $"{directory}/result.xml";
        resultXml.Save(resultPath);
    }
}
