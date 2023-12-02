namespace Presentation.Models.Project
{
    public class ProjectReference
    {
        public string ProjectPath { get; set; }

        public override string ToString()
        {
            return $"Project Path: {ProjectPath}";
        }
    }
}
