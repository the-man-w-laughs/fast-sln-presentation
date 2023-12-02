namespace Presentation.Models.SnlTree
{
    public class CsprojNode
    {
        public required string ProjectName { get; set; }
        public required string ProjectPath { get; set; }
        public required IEnumerable<ContentFile> SourceFiles { get; set; }
    }
}
