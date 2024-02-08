using FluentAssertions;
using Presentation.Services;

namespace Tests.ServiceTests;

public class CsprojParserTests
{
    [Fact]
    public void GetProjectInfo_ShouldParseValidCsproj()
    {
        // Arrange
        var filePath =
            "../../../../Tests/TestData/ServiceTests/CsprojParserTests/GetProjectInfo_ShouldParseValidCsproj/TestCsproj.csproj";
        var csprojContent = File.ReadAllText(filePath);

        var csprojParser = new CsprojParser();

        // Act
        var csprojInfo = csprojParser.GetProjectInfo(csprojContent);

        // Assert
        csprojInfo.Should().NotBeNull();
        csprojInfo.TargetFramework.Should().Be("net7.0");
        csprojInfo.PackageReferences.Should().HaveCount(4);
        csprojInfo.PackageReferences
            .Should()
            .ContainSingle(pr => pr.Name == "FluentAssertions" && pr.Version == "6.12.0");
        csprojInfo.PackageReferences
            .Should()
            .ContainSingle(pr => pr.Name == "Microsoft.NET.Test.Sdk" && pr.Version == "17.3.2");
        csprojInfo.PackageReferences
            .Should()
            .ContainSingle(pr => pr.Name == "xunit.runner.visualstudio" && pr.Version == "2.4.5");
        csprojInfo.PackageReferences
            .Should()
            .ContainSingle(pr => pr.Name == "coverlet.collector" && pr.Version == "3.1.2");
        csprojInfo.ProjectReferences.Should().HaveCount(1);
        csprojInfo.ProjectReferences
            .Should()
            .ContainSingle(pr => pr.ProjectPath == @"..\Presentation\Presentation.csproj");
    }
}
