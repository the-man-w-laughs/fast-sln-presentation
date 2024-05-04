namespace FastSlnPresentation.BLL.Models.Project
{
    public struct ProjectReference
    {
        public string ProjectPath { get; set; }

        public override string ToString()
        {
            return $"Project Path: {ProjectPath}";
        }
    }
}
