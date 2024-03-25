using FluentAssertions;
using Presentation.Services;

namespace Tests.ServiceTests;

public class SlnParserTests
{
    [Fact]
    public void GetSlnInfo_ShouldParseSolutionCorrectly()
    {
        var filePath =
            "../../../../Tests/TestData/ServiceTests/SlnParserTests/GetSlnInfo_ShouldParseSolutionCorrectly/TestSln.sln";
        var slnContent = File.ReadAllText(filePath);

        var slnParser = new SlnParser();

        // Act
        var slnInfo = slnParser.GetSlnInfo(slnContent);

        // Assert
        slnInfo.Should().NotBeNull();
        slnInfo.SlnProjectInfos.Should().NotBeNull().And.HaveCount(2);

        var projectInfo1 = slnInfo.SlnProjectInfos[0];
        projectInfo1.TypeGuid.Should().Be("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}");
        projectInfo1.Name.Should().Be("Presentation");
        projectInfo1.Path.Should().Be("Presentation\\Presentation.csproj");
        projectInfo1.ProjectGuid.Should().Be("{B50549BC-7F52-4E7F-B231-C50A0869D799}");

        var projectInfo2 = slnInfo.SlnProjectInfos[1];
        projectInfo2.TypeGuid.Should().Be("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}");
        projectInfo2.Name.Should().Be("Tests");
        projectInfo2.Path.Should().Be("Tests\\Tests.csproj");
        projectInfo2.ProjectGuid.Should().Be("{FCDB8E07-4271-4275-894F-23EE1137C63E}");
    }
}
