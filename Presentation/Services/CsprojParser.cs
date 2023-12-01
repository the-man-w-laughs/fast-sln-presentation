using System.Xml.Linq;
using Presentation.Models.Project;

namespace Presentation.Services;

public class CsprojParser
{
    public ProjectInfo GetProjectInfo(string csproj)
    {
        // Load the .csproj string as XML
        XDocument csprojXml = XDocument.Parse(csproj);

        // Initialize ProjectInfo
        ProjectInfo projectInfo = new ProjectInfo();

        // Access project properties
        XElement propertyGroup = csprojXml.Root?.Elements("PropertyGroup").FirstOrDefault();
        if (propertyGroup != null)
        {
            projectInfo.ProjectName = propertyGroup.Element("AssemblyName")?.Value;
            projectInfo.TargetFramework = propertyGroup.Element("TargetFramework")?.Value;
        }

        // Access package references
        IEnumerable<XElement> packageReferences = csprojXml.Root
            ?.Elements("ItemGroup")
            .Elements("PackageReference");
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
        IEnumerable<XElement> projectReferences = csprojXml.Root
            ?.Elements("ItemGroup")
            .Elements("ProjectReference");
        if (projectReferences != null)
        {
            foreach (var projectReference in projectReferences)
            {
                projectInfo.ProjectReferences.Add(
                    new ProjectReference
                    {
                        ProjectName = projectReference.Attribute("Include")?.Value
                    }
                );
            }
        }

        return projectInfo;
    }
}
