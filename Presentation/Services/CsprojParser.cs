using System.Xml.Linq;
using Presentation.Contracts;
using Presentation.Models.Project;

namespace Presentation.Services;

public class CsprojParser : ICsprojParser
{
    public CsprojInfo GetProjectInfo(string csproj)
    {
        // Load the .csproj string as XML

        if (csproj.Length > 0 && csproj[0] == '\uFEFF')
        {
            csproj = csproj.Substring(1);
        }

        var csprojXml = XDocument.Parse(csproj);

        // Initialize ProjectInfo
        var projectInfo = new CsprojInfo();

        // Access project properties
        var propertyGroup = csprojXml.Root?.Elements("PropertyGroup").FirstOrDefault();
        if (propertyGroup != null)
        {
            projectInfo.TargetFramework = propertyGroup.Element("TargetFramework")?.Value;
        }

        // Access package references
        var packageReferences = csprojXml.Root?.Elements("ItemGroup").Elements("PackageReference");
        if (packageReferences != null)
        {
            foreach (var packageReference in packageReferences)
            {
                projectInfo.PackageReferences.Add(
                    new PackageReference
                    {
                        Name = packageReference.Attribute("Include")?.Value,
                        Version = packageReference.Attribute("Version")?.Value
                    }
                );
            }
        }

        // Access project references
        var projectReferences = csprojXml.Root?.Elements("ItemGroup").Elements("ProjectReference");
        if (projectReferences != null)
        {
            foreach (var projectReference in projectReferences)
            {
                projectInfo.ProjectReferences.Add(
                    new ProjectReference
                    {
                        ProjectPath = projectReference.Attribute("Include")?.Value
                    }
                );
            }
        }

        return projectInfo;
    }
}
