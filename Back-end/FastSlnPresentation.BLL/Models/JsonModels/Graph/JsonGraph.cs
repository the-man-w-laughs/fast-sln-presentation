using System.Text.Json.Serialization;

namespace FastSlnPresentation.BLL.Models.Graph.GraphData
{
    public class JsonGraph
    {
        [JsonPropertyName("initialNodes")]
        public List<object> Nodes { get; set; }

        [JsonPropertyName("initialEdges")]
        public List<object> Edges { get; set; }

        public JsonGraph(List<object> nodes, List<object> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }
    }
}
