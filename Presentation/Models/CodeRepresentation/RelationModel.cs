namespace Presentation.Models.CodeRepresentation
{
    public struct RelationModel
    {
        public string SourceTypeName { get; set; }
        public string TargetTypeName { get; set; }
        public RelationType Type { get; set; }

        public RelationModel(string sourceTypeName, string targetTypeName, RelationType type)
        {
            SourceTypeName = sourceTypeName;
            TargetTypeName = targetTypeName;
            Type = type;
        }
    }
}
