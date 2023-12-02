namespace Presentation.Models.Project;

public class ProjectInfo
{
    public string ProjectName { get; set; }
    public string TargetFramework { get; set; }
    public List<PackageReference> PackageReferences { get; set; } = new List<PackageReference>();
    public List<ProjectReference> ProjectReferences { get; set; } = new List<ProjectReference>();

    public override string ToString()
    {
        return $"Project Name: {ProjectName}\nTarget Framework: {TargetFramework}\n"
            + $"Package References:\n{string.Join("\n", PackageReferences)}\n"
            + $"Project References:\n{string.Join("\n", ProjectReferences)}";
    }
}
