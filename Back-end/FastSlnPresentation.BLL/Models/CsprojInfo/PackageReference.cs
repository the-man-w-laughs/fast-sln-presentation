namespace FastSlnPresentation.BLL.Models.Project
{
    public struct PackageReference
    {
        public string Name { get; set; }
        public string Version { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Version: {Version}";
        }
    }
}
