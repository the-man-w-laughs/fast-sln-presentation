using System.Text.Json.Serialization;
using FastSlnPresentation.BLL.Models.JsonModels.Edges.Data;

namespace FastSlnPresentation.BLL.Models.JsonModels.Edges
{
    public class JsonEdge
    {
        [JsonPropertyName("id")]
        public string Id { get; }

        [JsonPropertyName("source")]
        public string Source { get; }

        [JsonPropertyName("target")]
        public string Target { get; }

        [JsonPropertyName("type")]
        public string Type { get; }

        [JsonPropertyName("data")]
        public EdgeData Data { get; }

        public JsonEdge(
            string id,
            string target,
            string source,
            string type,
            List<string>? label = null,
            List<string>? targetLabel = null,
            List<string>? sourceLabel = null
        )
        {
            Id = id;
            Target = target;
            Source = source;
            Type = type;
            Data = new EdgeData(label, targetLabel, sourceLabel);
        }
    }
}
