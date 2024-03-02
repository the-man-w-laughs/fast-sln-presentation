namespace Presentation.Models.JsonModels.Edges
{
    public class JsonEdge
    {
        public string Id { get; }
        public string Target { get; }
        public string Source { get; }
        public string Type { get; }

        public JsonEdge(string id, string target, string source, string type)
        {
            Id = id;
            Target = target;
            Source = source;
            Type = type;
        }
    }
}
