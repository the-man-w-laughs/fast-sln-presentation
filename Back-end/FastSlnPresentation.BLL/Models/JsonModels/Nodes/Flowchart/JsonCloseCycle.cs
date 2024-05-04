using System.Text.Json.Serialization;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes.Data;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes.Flowchart;

namespace FastSlnPresentation.BLL.Models.JsonModels
{
    public class JsonCloseCycle : INode, IFlowchartNode
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; } = "closeCycleNode";

        [JsonPropertyName("data")]
        public BlockData Data { get; set; }

        public JsonCloseCycle() { }

        public JsonCloseCycle(string id, string content)
        {
            var contentList = new List<string>() { content };
            Id = id;
            Data = new BlockData(contentList);
        }

        public JsonCloseCycle(string id, List<string> Content)
        {
            Id = id;
            Data = new BlockData(Content);
        }
    }
}
