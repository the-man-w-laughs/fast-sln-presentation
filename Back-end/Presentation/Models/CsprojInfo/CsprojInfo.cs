namespace Presentation.Models.Project;

public struct CsprojInfo
{
    public CsprojInfo(string? targetFramework)
    {
        TargetFramework = targetFramework;
        PackageReferences = new List<PackageReference>();
        ProjectReferences = new List<ProjectReference>();
    }

    public string? TargetFramework { get; set; }
    public List<PackageReference> PackageReferences { get; set; }
    public List<ProjectReference> ProjectReferences { get; set; }

    public override string ToString()
    {
        return $"Target Framework: {TargetFramework}\n"
            + $"Package References:\n{string.Join("\n", PackageReferences)}\n"
            + $"Project References:\n{string.Join("\n", ProjectReferences)}";
    }
}
