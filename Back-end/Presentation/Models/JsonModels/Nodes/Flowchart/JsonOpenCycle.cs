using System.Text.Json.Serialization;
using Presentation.Models.JsonModels.Nodes;
using Presentation.Models.JsonModels.Nodes.Data;
using Presentation.Models.JsonModels.Nodes.Flowchart;

namespace Presentation.Models.JsonModels
{
    public class JsonOpenCycle : INode, IFlowchartNode
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; } = "openCycleNode";

        [JsonPropertyName("data")]
        public BlockData Data { get; set; }

        public JsonOpenCycle() { }

        public JsonOpenCycle(string id, string content)
        {
            var contentList = new List<string>() { content };
            Id = id;
            Data = new BlockData(contentList);
        }

        public JsonOpenCycle(string id, List<string> content)
        {
            Id = id;
            Data = new BlockData(content);
        }
    }
}
