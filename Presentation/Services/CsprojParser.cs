using System.Xml.Linq;
using Presentation.Contracts;
using Presentation.Models.Project;

namespace Presentation.Services;

public class CsprojParser : ICsprojParser
{
    public CsprojInfo GetProjectInfo(string csproj)
    {
        if (string.IsNullOrWhiteSpace(csproj))
        {
            throw new ArgumentException("The provided csproj string is null or empty.");
        }

        if (csproj.Length > 0 && csproj[0] == '\uFEFF')
        {
            csproj = csproj.Substring(1);
        }

        var csprojXml = XDocument.Parse(csproj);

        if (csprojXml.Root == null)
        {
            throw new ArgumentException("Invalid csproj format: Root element not found.");
        }

        var propertyGroup = csprojXml.Root.Element("PropertyGroup");
        if (propertyGroup == null)
        {
            throw new ArgumentException("Invalid csproj format: PropertyGroup element not found.");
        }

        var targetFrameworkElement = propertyGroup.Element("TargetFramework");
        if (string.IsNullOrWhiteSpace(targetFrameworkElement?.Value))
        {
            throw new ArgumentException(
                "Invalid csproj format: TargetFramework is missing or empty."
            );
        }

        var projectInfo = new CsprojInfo(targetFrameworkElement.Value);

        // Access package references
        var packageReferences = csprojXml.Root.Elements("ItemGroup").Elements("PackageReference");
        foreach (var packageReference in packageReferences)
        {
            var name = packageReference.Attribute("Include")?.Value;
            var version = packageReference.Attribute("Version")?.Value;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentException(
                    "Invalid csproj format: PackageReference missing required attributes."
                );
            }

            projectInfo.PackageReferences.Add(
                new PackageReference { Name = name, Version = version }
            );
        }

        // Access project references
        var projectReferences = csprojXml.Root.Elements("ItemGroup").Elements("ProjectReference");
        foreach (var projectReference in projectReferences)
        {
            var projectPath = projectReference.Attribute("Include")?.Value;

            if (string.IsNullOrWhiteSpace(projectPath))
            {
                throw new ArgumentException(
                    "Invalid csproj format: ProjectReference missing required attributes."
                );
            }

            projectInfo.ProjectReferences.Add(new ProjectReference { ProjectPath = projectPath });
        }

        return projectInfo;
    }
}
