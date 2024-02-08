namespace Presentation.Models.SnlTree
{
    public class SlnTree
    {
        public required string SlnName { get; set; }
        public required string SlnPath { get; set; }
        public required List<CsprojNode> Projects { get; set; }
    }
}
