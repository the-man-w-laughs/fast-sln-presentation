using FastSlnPresentation.BLL.Models.Project;

namespace FastSlnPresentation.BLL.Models.SnlTree
{
    public class CsprojNode
    {
        public required string ProjectName { get; set; }
        public required string ProjectPath { get; set; }
        public required CsprojInfo CsprojInfo { get; set; }
        public required IEnumerable<ContentFile> SourceFiles { get; set; }
    }
}
