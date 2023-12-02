namespace Presentation.Models.Project;

public class CsprojInfo
{
    public string TargetFramework { get; set; }
    public List<PackageReference> PackageReferences { get; set; } = new List<PackageReference>();
    public List<ProjectReference> ProjectReferences { get; set; } = new List<ProjectReference>();

    public override string ToString()
    {
        return $"Target Framework: {TargetFramework}\n"
            + $"Package References:\n{string.Join("\n", PackageReferences)}\n"
            + $"Project References:\n{string.Join("\n", ProjectReferences)}";
    }
}
