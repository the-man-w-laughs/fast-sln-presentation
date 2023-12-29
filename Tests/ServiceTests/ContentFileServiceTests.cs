using Moq;
using Presentation.Contracts;
using Presentation.Models;
using Presentation.Services;
using Tests.ServiceTests.Helpers;

namespace Tests.ServiceTests;

public class ContentFileServiceTests
{
    private readonly ContentFileService _contentFileService;
    private readonly SlnParserHelper _slnParserHelper;
    private readonly CsprojParserHelper _csprojParserHelper;

    public ContentFileServiceTests()
    {
        var slnParser = new Mock<ISlnParser>();
        var csprojParser = new Mock<ICsprojParser>();
        _contentFileService = new ContentFileService(slnParser.Object, csprojParser.Object);

        _slnParserHelper = new SlnParserHelper(slnParser);
        _csprojParserHelper = new CsprojParserHelper(csprojParser);
    }

    [Fact]
    public void GetSnlTrees_ShouldReturnCorrectSlnTrees()
    {
        // Arrange

        var contentFiles = new List<ContentFile>
        {
            new("Sln1.sln", "Sln1"),
            new("Csproj1.csproj", "Csproj1"),
            new("Csproj2.csproj", "Csproj2"),
            new("Project1/File1.cs", "Project1/File1.cs"),
            new("Project1/File2.cs", "Project1/File2.cs"),
            new("Project2/File3.cs", "Project2/File3.cs")
        };

        // Act
        var slnTrees = _contentFileService.GetSnlTrees(contentFiles);

        // Assert
        Assert.NotNull(slnTrees);
        Assert.NotEmpty(slnTrees);

        foreach (var slnTree in slnTrees)
        {
            Assert.NotNull(slnTree);
            Assert.NotNull(slnTree.SlnName);
            Assert.NotNull(slnTree.SlnPath);
            Assert.NotNull(slnTree.Projects);
            Assert.NotEmpty(slnTree.Projects);

            foreach (var csprojNode in slnTree.Projects)
            {
                Assert.NotNull(csprojNode);
                Assert.NotNull(csprojNode.ProjectName);
                Assert.NotNull(csprojNode.ProjectPath);
                Assert.NotNull(csprojNode.CsprojInfo);
                Assert.NotNull(csprojNode.SourceFiles);
                Assert.NotEmpty(csprojNode.SourceFiles);
            }
        }
    }
}
