using System.Configuration;
using System.Diagnostics;
using Business.Octokit;
using Presentation.Services;

class SolutionParser
{
    static async Task Main()
    {
        var solutionFilePath =
            "/home/nazar/projects/diploma/fast-sln-presentation/FastSlnPresentation.sln";
        var solutionFileContent = File.ReadAllText(solutionFilePath);

        var solutionParser = new SlnParser();

        var projects = solutionParser.GetSlnProjectInfos(solutionFileContent);

        foreach (var project in projects)
        {
            Console.WriteLine(project);
            Console.WriteLine();
        }

        var projectFilePath =
            "/home/nazar/projects/diploma/fast-sln-presentation/Presentation/Presentation.csproj";
        var projectFileContent = File.ReadAllText(projectFilePath);

        var projectParser = new CsprojParser();

        var projectInfo = projectParser.GetProjectInfo(projectFileContent);

        Console.WriteLine(projectInfo);

        var pat = ConfigurationManager.AppSettings.Get("pat");
        var githubService = new GithubService(pat);

        var stopwatch = Stopwatch.StartNew();
        var allFiles = await githubService.GetAllFiles("the-man-w-laughs", "Obj-Renderer");
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
        Console.WriteLine(allFiles.Count);
    }
}
