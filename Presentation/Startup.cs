using Presentation.Services;

class SolutionParser
{
    static void Main()
    {
        var solutionFilePath = "/home/nazar/projects/diploma/Diploma-Research/DiplomaResearch.sln";
        var solutionFileContent = File.ReadAllText(solutionFilePath);

        var solutionParser = new SlnParser();

        var projects = solutionParser.GetSlnProjectInfos(solutionFileContent);

        foreach (var project in projects)
        {
            Console.WriteLine($"Project Name: {project.Name}");
            Console.WriteLine($"Project Type: {project.TypeGuid}");
            Console.WriteLine($"Project Guid: {project.ProjectGuid}");
            Console.WriteLine($"Project Path: {project.Path}");
            Console.WriteLine();
        }
    }
}
