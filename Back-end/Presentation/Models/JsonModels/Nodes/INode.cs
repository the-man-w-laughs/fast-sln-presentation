namespace Presentation.Models.JsonModels.Nodes
{
    public interface INode
    {
        public string Id { get; set; }
        public string Type { get; }
    }
}
