using System.Text.Json.Serialization;

namespace Presentation.Models.JsonModels.Edges
{
    public class JsonEdge
    {
        [JsonPropertyName("id")]
        public string Id { get; }

        [JsonPropertyName("target")]
        public string Target { get; }

        [JsonPropertyName("source")]
        public string Source { get; }

        [JsonPropertyName("type")]
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
