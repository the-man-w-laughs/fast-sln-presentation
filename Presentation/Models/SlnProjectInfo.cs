namespace Presentation.Models
{
    public struct SlnProjectInfo
    {
        public string TypeGuid { get; set; }
        public string Name { get; set; }
        public string ProjectGuid { get; set; }
        public string Path { get; set; }

        public override string ToString() =>
            $"TypeGuid: {TypeGuid}\nName: {Name}\nProjectGuid: {ProjectGuid}\nPath: {Path}";
    }
}
