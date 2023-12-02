namespace Presentation.Models.Project;

public class ProjectInfo
{
    public string ProjectName { get; set; }
    public string TargetFramework { get; set; }
    public List<PackageReference> PackageReferences { get; set; } = new List<PackageReference>();
    public List<ProjectReference> ProjectReferences { get; set; } = new List<ProjectReference>();
}
