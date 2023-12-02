namespace Presentation.Models.Project
{
    public class PackageReference
    {
        public string Name { get; set; }
        public string Version { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Version: {Version}";
        }
    }
}
