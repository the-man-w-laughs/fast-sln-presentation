using System.Configuration;
using System.Diagnostics;
using Business.Octokit;
using Presentation.Services;

class SolutionParser
{
    static async Task Main()
    {
        var solutionParser = new SlnParser();
        var projectParser = new CsprojParser();

        var pat = ConfigurationManager.AppSettings.Get("pat");
        var githubService = new GithubService(pat);

        var stopwatch = Stopwatch.StartNew();
        var allFiles = await githubService.GetAllFiles("the-man-w-laughs", "Obj-Renderer");
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
        Console.WriteLine(allFiles.Count);

        var contentFileService = new ContentFileService(solutionParser, projectParser);

        var slnTrees = contentFileService.GetSnlTrees(allFiles);
    }
}
