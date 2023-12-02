using System.Text.RegularExpressions;
using Presentation.Contracts;
using Presentation.Models;
using Presentation.Models.SnlTree;
using Presentation.Exceptions;

namespace Presentation.Services
{
    public class ContentFileService : IContentFileService
    {
        private readonly ISlnParser _slnParser;
        private readonly ICsprojParser _csprojParser;

        public ContentFileService(ISlnParser slnParser, ICsprojParser csprojParser)
        {
            _slnParser = slnParser;
            _csprojParser = csprojParser;
        }

        public List<SlnTree> GetSnlTrees(IEnumerable<ContentFile> contentFiles)
        {
            // Filter files to get only solution files
            var solutionFiles = contentFiles.Where(file => file.Path.EndsWith(".sln")).ToList();

            var slnTrees = new List<SlnTree>();

            foreach (var solutionFile in solutionFiles)
            {
                var slnTree = new SlnTree()
                {
                    SlnName = Path.GetFileNameWithoutExtension(solutionFile.Path),
                    SlnPath = Path.GetDirectoryName(solutionFile.Path)!,
                    Projects = new List<CsprojNode>()
                };

                // Parse information from the solution file content
                var slnInfo = _slnParser.GetSlnProjectInfos(solutionFile.Content);

                foreach (var slnProjectInfo in slnInfo.SlnProjectInfos)
                {
                    // Get absolute paths and project file
                    var slnDirectoryPath = Path.GetDirectoryName(solutionFile.Path)!;
                    var projectPath = NormalizePath(slnProjectInfo.Path);
                    string absoluteProjectPath = Path.Combine(slnDirectoryPath, projectPath);

                    // Find the content file corresponding to the project
                    var projectFile = contentFiles.FirstOrDefault(
                        file => file.Path.Equals(absoluteProjectPath)
                    );

                    if (projectFile == null)
                    {
                        throw new NotFoundException($"Project not found:\n{slnProjectInfo}");
                    }

                    var absoluteProjectDirectoryPath = Path.GetDirectoryName(absoluteProjectPath)!;

                    // Get source files within the project directory
                    var projectSourceFiles = contentFiles.Where(
                        file =>
                            file.Path.StartsWith(absoluteProjectDirectoryPath)
                            && file.Path.EndsWith(".cs")
                    );

                    // Create a CsprojNode with project information
                    var csprojNode = new CsprojNode
                    {
                        ProjectName = Path.GetFileNameWithoutExtension(
                            absoluteProjectDirectoryPath
                        ),
                        ProjectPath = absoluteProjectDirectoryPath,
                        SourceFiles = new List<ContentFile>(projectSourceFiles)
                    };

                    // Add the CsprojNode to the list of projects in the SlnTree
                    slnTree.Projects.Add(csprojNode);
                }

                // Add the SlnTree to the list of SlnTrees
                slnTrees.Add(slnTree);
            }

            return slnTrees;
        }

        // Utility method to normalize paths (convert backslashes and multiple slashes to single forward slashes)
        static string NormalizePath(string path)
        {
            return Regex.Replace(path, @"[\\/]+", "/");
        }
    }
}
