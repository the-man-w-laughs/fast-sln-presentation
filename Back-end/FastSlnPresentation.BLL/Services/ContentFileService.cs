using System.Text.RegularExpressions;
using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Models;
using FastSlnPresentation.BLL.Models.SnlTree;
using FastSlnPresentation.BLL.Exceptions;

namespace FastSlnPresentation.BLL.Services
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
                    SlnPath = solutionFile.Path,
                    Projects = new List<CsprojNode>()
                };

                // Parse information from the solution file content
                var slnInfo = _slnParser.GetSlnInfo(solutionFile.Content);

                foreach (var slnProjectInfo in slnInfo.SlnProjectInfos)
                {
                    // Get absolute paths and project file
                    var slnDirectoryPath = $"/{Path.GetDirectoryName(solutionFile.Path)}";
                    var projectPath = NormalizePath(slnProjectInfo.Path);
                    var absoluteProjectPath = Path.GetFullPath(projectPath, slnDirectoryPath);

                    // Find the content file corresponding to the project
                    var projectFile = contentFiles.FirstOrDefault(
                        file => file.Path.Equals(absoluteProjectPath.Substring(1))
                    );

                    if (projectFile.Equals(default(ContentFile)))
                    {
                        throw new NotFoundException($"Project not found:\n{slnProjectInfo}");
                    }

                    var absoluteProjectDirectoryPath = Path.GetDirectoryName(absoluteProjectPath)!;

                    // Get source files within the project directory
                    var projectSourceFiles = contentFiles.Where(
                        file =>
                            file.Path.StartsWith(absoluteProjectDirectoryPath.Substring(1))
                            && file.Path.EndsWith(".cs")
                    );

                    // parse csproj file
                    var csprojInfo = _csprojParser.GetProjectInfo(projectFile.Content);

                    // update project references paths to absolute paths
                    csprojInfo.ProjectReferences.ForEach(
                        reference =>
                            reference.ProjectPath = Path.GetFullPath(
                                    NormalizePath(reference.ProjectPath),
                                    absoluteProjectDirectoryPath
                                )
                                .Substring(1)
                    );

                    // Create a CsprojNode with project information
                    var csprojNode = new CsprojNode
                    {
                        ProjectName = Path.GetFileNameWithoutExtension(absoluteProjectPath),
                        ProjectPath = absoluteProjectPath.Substring(1),
                        CsprojInfo = csprojInfo,
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
        private string NormalizePath(string path)
        {
            return Regex.Replace(path, @"[\\/]+", "/");
        }
    }
}
